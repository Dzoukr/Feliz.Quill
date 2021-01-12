module Docs.Pages.Themes

open Fable.Core
open Feliz
open Feliz.Bulma
open Feliz.Quill
open Shared

let view =
    Html.div [
        prop.key "themes-page"
        prop.children [
            Bulma.title "Themes"
            Html.hr []
            Bulma.content [
                Html.p "There are two predefined themes - Snow and Bubble."
                Bulma.title.h4 "Snow (default)"
                Quill.editor [
                    editor.theme Theme.Snow
                    editor.toolbar Toolbar.medium
                    editor.placeholder "Start some awesome story..."
                ]
            ]
            Bulma.content [
                Bulma.title.h4 "Bubble"
                Quill.editor [
                    editor.theme Theme.Bubble
                    editor.toolbar Toolbar.medium
                    editor.placeholder "Write something here and then select text to see the Medium-like bubble"
                ]
            ]
            Bulma.content [ Html.p "To select which one to use you can simply set the theme property." ]
            Bulma.content [
                code """Quill.editor [
    editor.theme Theme.Bubble
    editor.placeholder "Start some awesome story..."
]"""
            ]

        ]
    ]
