namespace Feliz.Quill

open System
open Feliz
open Fable.Core
open Fable.Core.JsInterop

module private Props =
    let inline setDefault<'a> (name:string, value:obj) (props:List<'a>) =
        let found =
            props
            |> List.map unbox<string * _>
            |> List.exists (fun (n,_) -> n = name)
        match found with
        | true -> props
        | false -> (unbox (name, value)) :: props

[<Erase>]
type Quill =
    static member inline editor (props: IQuillEditorProperty list) =
        let safeProps =
            props
            |> Props.setDefault ("theme", Theme.Snow)
        Editor.Editor (unbox<Editor.Props> (createObj !!safeProps))
