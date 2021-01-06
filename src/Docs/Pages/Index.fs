module Docs.Pages.Index

open Feliz
open Feliz.Bulma
open Feliz.Quill

let editorContent = """
    <p><img src="https://camo.githubusercontent.com/3e9a6019c30cfb802c984ae1ea64d738cdf02ccc8136ea8778d3d1f1510ae64b/68747470733a2f2f7175696c6c6a732e636f6d2f6173736574732f696d616765732f6c6f676f2e737667" height="211.59656708595386" width="356" data-align="center" style="display: block; margin: auto;"></p>
    <h2>Welcome to amazing Feliz.Quill editor component</h2>
    <p>Text editation for Feliz projects is now <strong>easier</strong> than ever!</p>
    <p>If you don't trust me, <i>highlight this text</i> and you'll see yourself.</p>
    <p>Of course it's not only about text. You can embed video as well!</p>
    <p><iframe class="ql-video" frameborder="0" allowfullscreen="true" src="https://www.youtube.com/embed/FssULNGSZIA?showinfo=0" height="328" width="656" data-align="center" style="display: block; margin: auto;"></iframe></p>
"""

let fullContent = """
    <h2>Do you like non-Medium editors?</h2>
    <p>Just change on property in configration and you're all set!</p>

"""

let view =
    React.fragment [
        Bulma.title [
            Html.text "Feliz.Quill "
            Html.a [
                prop.href "https://www.nuget.org/packages/Feliz.Quill/"
                prop.children [
                    Html.img [
                        prop.src "https://img.shields.io/nuget/v/Feliz.Quill.svg?style=flat-square"
                    ]
                ]
            ]
        ]
        Bulma.subtitle [
            Html.text "Powerfull rich text editor "
            Html.a [ prop.href "https://quilljs.com/"; prop.text "Quill" ]
            Html.text " as Feliz component"
        ]
        Html.hr []
        Bulma.content [
            Html.div [
                prop.className "ql-reset"
                prop.children [
                    Quill.editor [
                        editor.onTextChanged (fun x -> Fable.Core.JS.console.log(x))
                        editor.theme Theme.Bubble
                        editor.toolbar Toolbar.medium
                        editor.placeholder "Write something cool"
                        editor.defaultValue editorContent
                    ]
                ]
            ]
        ]
        Bulma.content [
            Html.p "But hey! What about good old editor feeling? Say no more!"
        ]
        Bulma.content [
            Html.div [
                prop.children [
                    Quill.editor [
                        editor.onTextChanged (fun x -> Fable.Core.JS.console.log(x))
                        editor.theme Theme.Snow
                        editor.toolbar Toolbar.all
                        editor.defaultValue fullContent
                    ]
                ]
            ]
        ]
    ]
