module Docs.Pages.QuickStart

open Fable.Core
open Feliz
open Feliz.Bulma
open Feliz.Quill
open Shared

let view =
    Html.div [
        Bulma.title "Quickstart"
        Html.hr []
        Bulma.content [
            Html.p "To create such editor..."
            Quill.editor [
                editor.onTextChanged (fun x -> Fable.Core.JS.console.log(x))
                editor.toolbar Toolbar.all
                editor.placeholder "Start some awesome story..."
            ]
        ]
        Bulma.content [ Html.p "...simply use this code" ]
        Bulma.content [
            code """open Feliz.Quill

Quill.editor [
    editor.onTextChanged (fun x -> Fable.Core.JS.console.log(x))
    editor.toolbar Toolbar.all
    editor.placeholder "Start some awesome story..."
]"""
        ]
        Bulma.content [
            Html.p "Proceed to next documentation section to learn more about Toolbars"
        ]
    ]
