module Feliz.Quill.JsInterop

open Fable.Core.JsInterop

type Props = Map<string,obj>

module Props =
    let inline ofList (p:List<'a>) : Props =
        p
        |> List.map unbox<string * obj>
        |> Map.ofList

    let inline ofSeq (p:'a seq) : Props =
        p
        |> Seq.map unbox<string * obj>
        |> Map.ofSeq

    let inline singleton (p:'a) : Props =
        p
        |> List.singleton
        |> ofList

    let inline get<'a> (name:string) (p:Props) = name |> p.TryFind |> Option.map (fun x -> x :?> 'a)
    let inline getDefault<'a> (name:string) (v:'a) (p:Props) = p |> get name |> Option.defaultValue v
    let inline create<'a,'b> (safeProps:'b list) = unbox<'a> (createObj !!safeProps)
    let inline setDefault<'a> (name:string, value:obj) (props:List<'a>) =
        let found =
            props
            |> List.map unbox<string * _>
            |> List.exists (fun (n,_) -> n = name)
        match found with
        | true -> props
        | false -> (unbox (name, value)) :: props
