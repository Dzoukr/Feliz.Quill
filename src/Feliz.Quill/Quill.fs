namespace Feliz.Quill

open System
open Feliz
open Fable.Core
open Feliz.Quill.JsInterop

[<Erase>]
type Quill =
    static member inline editor (props: IQuillEditorProperty list) =
        props
        |> Props.setDefault ("theme", Theme.Snow)
        |> Props.ofList
        |> Editor.Editor
