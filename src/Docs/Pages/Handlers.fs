module Docs.Pages.Handlers

open Fable.Core
open Feliz
open Feliz.Bulma
open Feliz.Quill
open Shared

[<Import("myHandler","./MyHandler.js")>]
let private myFSharpHandler: HandlerFunction = jsNative

let view =
    Html.div [
        prop.key "handlers-page"
        prop.children [
            Bulma.title "Handlers"
            Html.hr []
            Bulma.content [
                Html.p [ prop.dangerouslySetInnerHTML "To customize Feliz.Quill behavior, you can use feature called <a href='https://quilljs.com/docs/modules/toolbar/#handlers'>Handlers.</a>"]
                Html.p [ prop.dangerouslySetInnerHTML "To make your life easier, there is one built-in handy handler called <i>imageFromUrl</i> which lets you use image url instead of base64 data:image." ]
                Bulma.title.h4 "imageFromUrl"
                Quill.editor [
                    editor.toolbar Toolbar.medium
                    editor.placeholder "Try to insert some image to see how behavior changed"
                    editor.handlers [
                        Handler.imageFromUrl
                    ]
                ]
            ]

            Bulma.content [ Html.p "To select which one to use you can simply set the theme property." ]
            Bulma.content [
                code """Quill.editor [
    editor.toolbar Toolbar.medium
    editor.placeholder "Try to insert some image to see how behavior changed"
    editor.handlers [
        Handler.imageFromUrl // <-- this will change image insertion behavior
    ]
]"""
            ]
            Bulma.content [
                Bulma.title.h4 "Custom handlers"
                Html.p "If you need you can write and register your own handler."

                Bulma.title.h5 "MyHandler.js"
                code """export function myHandler(_) {
    alert("Hi from my custom handler!")
}
"""

                Bulma.title.h5 "Importing handler from .js to F#"
                code """[<Import("myHandler","./MyHandler.js")>]
let myFSharpHandler: HandlerFunction = jsNative
"""
                Bulma.title.h5 "Registering handler"
                code """Quill.editor [
    editor.toolbar Toolbar.medium
    editor.placeholder "Try to insert some image to see your fancy handler"
    editor.handlers [
        Handler.custom "image" myFSharpHandler
    ]
]
"""
            ]

            Bulma.content [
                Bulma.title.h5 "Output"
                Quill.editor [
                    editor.toolbar Toolbar.medium
                    editor.placeholder "Try to insert some image to see your fancy handler"
                    editor.handlers [
                        Handler.custom "image" myFSharpHandler
                    ]
                ]
            ]

        ]
    ]
