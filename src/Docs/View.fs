module Docs.View

open Feliz
open Feliz.Bulma
open Feliz.UseElmish
open Docs.Router
open Feliz.Router
open Docs.State

let menuPart (model:Model) =
    let item (t:string) p =
        let isActive =
            if model.CurrentPage = p then [ helpers.isActive; color.hasBackgroundPrimary ] else []
        Bulma.menuItem.a [
            yield! isActive
            yield prop.text t
            yield prop.href (Router.getHref p)
        ]

    Bulma.menu [
        Bulma.menuLabel "General"
        Bulma.menuList [
            item "Overview" Index
            item "Installation" Installation
            item "Quickstart" QuickStart
            item "Toolbars" Toolbars
            item "Themes" Themes
            item "Handlers" Handlers
        ]
    ]


let contentPart model =
    match model.CurrentPage with
    | Index -> Pages.Index.view
    | Installation -> Pages.Installation.view
    | QuickStart -> Pages.QuickStart.view
    | Toolbars -> Pages.Toolbars.view
    | Themes -> Pages.Themes.view
    | Handlers -> Pages.Handlers.view


let private faIcon (cn:string) = Html.i [ prop.className cn ]
let private faIconLink (cn:string) (href:string) =
    Html.a [
        prop.href href
        prop.children [ faIcon cn ]
    ]


let footer =
    Bulma.footer [
        Bulma.container [
            Html.div [
                prop.className "icons"
                prop.children [
                    Html.a [
                        prop.href "https://github.com/Dzoukr/Feliz.Quill"
                        prop.children [
                            faIcon "fab fa-github"
                            Html.span "Source code on GitHub (MIT license)"
                        ]
                    ]
                ]
            ]
        ]
    ]

[<ReactComponent>]
let AppView () =
    let model, dispatch = React.useElmish(init, update, [| |])
    let render =
        React.fragment [
            Bulma.container [
                Bulma.section [
                    Bulma.columns [
                        Bulma.column [
                            column.is2
                            prop.children (menuPart model)
                        ]
                        Bulma.column (contentPart model)
                    ]
                ]
            ]
            footer
        ]
    React.router [
        router.onUrlChanged (parseUrl >> UrlChanged >> dispatch)
        router.children render
    ]
