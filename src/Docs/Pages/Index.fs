module Docs.Pages.Index

open Feliz
open Feliz.Bulma
open Feliz.Quill

let view =
    Bulma.content [
        Quill.editor [
            editor.onTextChanged (fun x -> Fable.Core.JS.console.log(x))
            editor.theme Theme.Snow
            editor.toolbar Toolbar.medium
            editor.placeholder "Write something cool"
            editor.defaultValue "<b>Hello</b>"
        ]
    ]
