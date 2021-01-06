namespace Feliz.Quill

open Fable.Core
open Fable.Core.JsInterop
open Fable.React
open Feliz

type Theme = Snow | Bubble

type ToolbarHeader =
    | Button of int
    | Dropdown of int list

module private ToolbarHeader =
    let toJsObj = function
        | Button v -> createObj [ "header" ==> v ]
        | Dropdown vs ->
            vs
            |> List.map box
            |> List.append [ box false ]
            |> List.toArray
            |> (fun x -> createObj [ "header" ==> x ])

type ToolbarItem =
    | Font of string list
    | Header of ToolbarHeader
    | Bold
    | Italic
    | Underline
    | Strikethrough
    | Blockquote
    | Code
    | ForegroundColor
    | BackgroundColor
    | Superscript
    | Subscript
    | OrderedList
    | UnorderedList
    | IncreaseIndent
    | DecreaseIndent
    | CodeBlock
    | Link
    | Image
    | Video
    | Align
    | Direction
    | Clean

module private ToolbarItem =
    let toJsObj = function
        | Font fonts -> createObj [ "font" ==> (fonts |> List.toArray) ]
        | Header head -> head |> ToolbarHeader.toJsObj
        | Bold -> "bold" |> box
        | Italic -> "italic" |> box
        | Underline -> "underline" |> box
        | Strikethrough -> "strike" |> box
        | Blockquote -> "blockquote" |> box
        | Code -> "code" |> box
        | ForegroundColor -> createObj [ "color" ==> [||] ]
        | BackgroundColor -> createObj [ "background" ==> [||] ]
        | Superscript -> createObj [ "script" ==> "super" ]
        | Subscript -> createObj [ "script" ==> "sub" ]
        | OrderedList -> createObj [ "list" ==> "ordered" ]
        | UnorderedList -> createObj [ "list" ==> "bullet" ]
        | IncreaseIndent -> createObj [ "indent" ==> "+1" ]
        | DecreaseIndent -> createObj [ "indent" ==> "-1" ]
        | CodeBlock -> "code-block" |> box
        | Link -> "link" |> box
        | Image -> "image" |> box
        | Video -> "video" |> box
        | Align -> createObj [ "align" ==> [||] ]
        | Direction -> createObj [ "direction" ==> "rtl" ]
        | Clean -> "clean" |> box

type ToolbarSection = ToolbarItem list

module private ToolbarSection =
    let toJsObj = List.map ToolbarItem.toJsObj >> List.toArray

type Toolbar = ToolbarSection list

module Toolbar =
    let internal toJsObj = List.map ToolbarSection.toJsObj >> List.toArray

    let all =
        [
            [ Font [ "sans-serif"; "serif"; "monospace"; ]; Header (ToolbarHeader.Button 1); Header (ToolbarHeader.Button 2) ]
            [ Bold; Italic; Underline; Strikethrough; Blockquote; Code ]
            [ ForegroundColor; BackgroundColor ]
            [ Subscript; Superscript ]
            [ OrderedList; UnorderedList; DecreaseIndent; IncreaseIndent; CodeBlock ]
            [ Link; Image; Video ]
            [ Align; Direction ]
            [ Clean ]
        ]

    let medium =
        [
            [ Bold; Italic; Link ]
            [ Header (ToolbarHeader.Button 1); Header (ToolbarHeader.Button 2); Blockquote ]
            [ Image; Video ]
        ]

module private Theme =
    let value = function
        | Snow -> "snow"
        | Bubble -> "bubble"

[<RequireQualifiedAccess; System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)>]
module Editor =
    type Props =
        abstract onTextChanged : (string -> unit) option
        abstract theme : Theme
        abstract placeholder : string option
        abstract defaultValue : string option
        abstract toolbar : Toolbar option

    type QuillResize = obj
    type Quill =
        abstract register : string -> QuillResize -> unit
        abstract import : string -> unit

    [<ImportDefault(from="quill")>]
    let quill: Quill = jsNative

    [<ImportDefault(from="quill-image-resize-module-react")>]
    let imageResize: obj = jsNative


    [<ReactComponent>]
    let Editor (p:Props) =
        quill.register "modules/imageResize" imageResize
        ofImport
            "default"
            "react-quill"
            {|
                onChange = p.onTextChanged
                placeholder = p.placeholder
                theme = p.theme |> Theme.value
                defaultValue = p.defaultValue
                modules =
                    {|
                       toolbar = p.toolbar |> Option.defaultValue Toolbar.all |> Toolbar.toJsObj
                       imageResize = {| parchment = quill.import("parchment") |}
                    |}
            |}
            []


type IQuillEditorProperty = interface end

[<Erase>]
type editor =
    static member inline onTextChanged (eventHandler: string -> unit) : IQuillEditorProperty = unbox ("onTextChanged", eventHandler)
    static member inline theme (theme:Theme) : IQuillEditorProperty = unbox ("theme", theme)
    static member inline placeholder (text:string) : IQuillEditorProperty = unbox ("placeholder", text)
    static member inline defaultValue (text:string) : IQuillEditorProperty = unbox ("defaultValue", text)
    static member inline toolbar (toolbar:Toolbar) : IQuillEditorProperty = unbox ("toolbar", toolbar)

