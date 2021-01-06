module Docs.Pages.Installation

open Feliz
open Feliz.Bulma
open Shared

let view =
    Html.div [
        Bulma.title "Installation"
        Html.hr []
        Bulma.content [
            Bulma.title.h4 "Using Femto (recommended)"
            Html.p [ prop.dangerouslySetInnerHTML "The easiest way is to use <a href='https://github.com/zaid-ajaj/femto'>Femto CLI</a> which will take care of all dependencies including npm libraries." ]
            code ("femto install Feliz.Quill")
        ]
        Bulma.content [
            Bulma.title.h4 "Manual"
            Html.p "If you want to install this package manually, use usual NuGet package command"
            code ("Install-Package Feliz.Quill")
            Html.p "or using Paket"
            code ("paket add Feliz.Quill")
            Html.p "Please don't forget that this library has also dependencies on frontend packages, so you need to add it to package.json file using yarn / npm command"
            ["react-quill@2.0.0-beta.2";"quill-blot-formatter@1.0.5"]
            |> List.map (fun n -> sprintf "yarn add %s" n)
            |> String.concat "\n" |> code
        ]
        Bulma.content [
            Bulma.title.h4 "CSS styles"
            Html.p "This component requires additional scss styles to be loaded. Please don't forget to add import into your style sheet:"
            code ("""@import "~react-quill/dist/quill.snow.css";
@import "~react-quill/dist/quill.bubble.css";
@import "~react-quill/dist/quill.core.css";""")
        ]
    ]

