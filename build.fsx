open Fake.IO

#r "paket: groupref Build //"
#load ".fake/build.fsx/intellisense.fsx"

open System.IO
open Fake.Core
open Fake.DotNet
open Fake.IO.FileSystemOperators
open Fake.Core.TargetOperators

module Tools =
    let private findTool tool winTool =
        let tool = if Environment.isUnix then tool else winTool
        match ProcessUtils.tryFindFileOnPath tool with
        | Some t -> t
        | _ ->
            let errorMsg =
                tool + " was not found in path. " +
                "Please install it and make sure it's available from your path. "
            failwith errorMsg

    let private runTool (cmd:string) args workingDir =
        let arguments = args |> String.split ' ' |> Arguments.OfArgs
        Command.RawCommand (cmd, arguments)
        |> CreateProcess.fromCommand
        |> CreateProcess.withWorkingDirectory workingDir
        |> CreateProcess.ensureExitCode
        |> Proc.run
        |> ignore

    let dotnet cmd workingDir =
        let result =
            DotNet.exec (DotNet.Options.withWorkingDirectory workingDir) cmd ""
        if result.ExitCode <> 0 then failwithf "'dotnet %s' failed in %s" cmd workingDir

    let femto args = args |> sprintf "femto %s" |> dotnet
    let node = runTool (findTool "node" "node.exe")
    let yarn = runTool (findTool "yarn" "yarn.cmd")

let docsSrcPath = "src" </> "Docs"
let docsPublishPath = "publish" </> "docs"
let fableBuildPath = ".fable-build"

// Targets
let clean proj = [ proj </> "bin"; proj </> "obj" ] |> Shell.cleanDirs

let validateFemto proj = Tools.femto "--validate" proj

let createNuget proj =
    clean proj
    validateFemto proj
    Tools.yarn "install" proj
    Tools.dotnet "restore --no-cache" proj
    Tools.dotnet "pack -c Release" proj

let publishNuget proj =
    createNuget proj
    let nugetKey =
        match Environment.environVarOrNone "NUGET_KEY" with
        | Some nugetKey -> nugetKey
        | None -> failwith "The Nuget API key must be set in a NUGET_KEY environmental variable"
    let nupkg =
        Directory.GetFiles(proj </> "bin" </> "Release")
        |> Seq.head
        |> Path.GetFullPath
    Tools.dotnet (sprintf "nuget push %s -s nuget.org -k %s" nupkg nugetKey) proj

Target.create "Pack" (fun _ -> "src" </> "Feliz.Quill" |> createNuget)
Target.create "Publish" (fun _ -> "src" </> "Feliz.Quill" |> publishNuget)

Target.create "InstallDocs" (fun _ ->
    printfn "Node version:"
    Tools.node "--version" docsSrcPath
    printfn "Yarn version:"
    Tools.yarn "--version" docsSrcPath
    Tools.yarn "install --frozen-lockfile" docsSrcPath
)

Target.create "PublishDocs" (fun _ ->
    [ docsPublishPath] |> Shell.cleanDirs
    Tools.dotnet (sprintf "fable --outDir %s --run webpack-cli -p" fableBuildPath) docsSrcPath
)

Target.create "RunDocs" (fun _ -> Tools.dotnet (sprintf "fable watch --outDir %s --run webpack-dev-server" fableBuildPath) docsSrcPath)

"InstallDocs" ==> "RunDocs"
"InstallDocs" ==> "PublishDocs"

Target.runOrDefaultWithArguments "RunDocs"
