namespace Feliz.Quill

open Browser.Types
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

type HandlerFunction = obj -> unit

type Handler = {
    Name : string
    Function : HandlerFunction
}

[<RequireQualifiedAccess>]
module Handler =
    [<Import("imageFromUrl","./CustomHandlers.js")>]
    let private imageUrlHandlerFunction: HandlerFunction = jsNative

    let custom name func = { Name = name; Function = func }

    let imageFromUrl = {
        Name = "image"
        Function = imageUrlHandlerFunction
    }

[<RequireQualifiedAccess; System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)>]
module Editor =
    type Props =
        abstract onTextChanged : (string -> unit) option
        abstract theme : Theme
        abstract placeholder : string option
        abstract defaultValue : string option
        abstract toolbar : Toolbar option
        abstract handlers : Handler list option
        abstract formats : string []

    type Quill =
        abstract register : string * obj * bool -> unit
        abstract register : obj * bool -> unit

    [<ImportDefault("quill")>]
    let quill: Quill = jsNative

    [<ImportDefault("quill-blot-formatter")>]
    let blotFormatterJs: obj = jsNative

    [<Import("ImageBlot","./CustomBlots.js")>]
    let imageBlot: obj = jsNative

    [<Import("VideoBlot","./CustomBlots.js")>]
    let videoBlot: obj = jsNative

    quill.register("modules/blotFormatter", blotFormatterJs, true)
    quill.register(imageBlot, true)
    quill.register(videoBlot, true)

    [<ReactComponent>]
    let Editor (p:Props) =

        // Fix bug with adding more and more blot formatter proxy images
        // I tried React.useEffect but it removed also valid images  ¯\_(ツ)_/¯
        let nodes = Browser.Dom.window.document.querySelectorAll(".blot-formatter__proxy-image")
        if nodes.length > 0 then
            for i in [0..nodes.length - 1] do
                let el = nodes.Item i
                el.remove()

        let handlers =
            p.handlers
            |> Option.bind (fun x -> if x.Length = 0 then None else Some x)
            |> Option.map (fun x ->
                createObj [
                    for h in x do
                    h.Name ==> h.Function
                ]
            )

        ofImport
            "default"
            "react-quill"
            {|
                onChange = p.onTextChanged
                placeholder = p.placeholder
                theme = p.theme |> Theme.value
                defaultValue = p.defaultValue
                formats = p.formats
                modules =
                    createObj [
                        "toolbar" ==>
                            createObj [
                                "container" ==> (p.toolbar |> Option.defaultValue Toolbar.all |> Toolbar.toJsObj)
                                match handlers with
                                | Some h ->
                                    "handlers" ==> h
                                | None -> ()
                            ]

                        "blotFormatter" ==> {| |}
                    ]
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
    static member inline handlers (handlers:Handler list) : IQuillEditorProperty = unbox ("handlers", handlers)
    static member inline formats (formats:string list) : IQuillEditorProperty = unbox ("formats", formats |> Array.ofList)
