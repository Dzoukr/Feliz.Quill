module Docs.Pages.Toolbars

open Fable.Core
open Feliz
open Feliz.Bulma
open Feliz.Quill
open Shared

let view =
    Html.div [
        Bulma.title "Toolbars"
        Html.hr []
        Bulma.content [
            Html.p "There are two predefined sets of Toolbars - All and Medium."
            Bulma.title.h4 "Medium (default)"
            Quill.editor [
                editor.toolbar Toolbar.medium
                editor.placeholder "Start some awesome story..."
            ]
        ]
        Bulma.content [
            Bulma.title.h4 "All"
            Quill.editor [
                editor.toolbar Toolbar.all
                editor.placeholder "Start some awesome story..."
            ]
        ]
        Bulma.content [ Html.p "But you can define your own" ]
        Bulma.content [
            Quill.editor [
                editor.toolbar [
                    [ Link; Image; Video ]
                    [ Header (ToolbarHeader.Dropdown [1..4]) ]
                    [ ForegroundColor; BackgroundColor ]
                    [ Bold; Italic; Underline; Strikethrough; Blockquote; Code ]
                    [ OrderedList; UnorderedList; DecreaseIndent; IncreaseIndent; CodeBlock ]
                ]
                editor.placeholder "Start some awesome story..."
            ]
        ]
        Bulma.content [
            code """Quill.editor [
    editor.toolbar [
        [ Link; Image; Video ]
        [ Header (ToolbarHeader.Dropdown [1..4]) ]
        [ ForegroundColor; BackgroundColor ]
        [ Bold; Italic; Underline; Strikethrough; Blockquote; Code ]
        [ OrderedList; UnorderedList; DecreaseIndent; IncreaseIndent; CodeBlock ]
    ]
    editor.placeholder "Start some awesome story..."
]"""
        ]
        Bulma.content [
            Html.p "Proceed to next documentation section to learn more about Themes"
        ]
    ]
