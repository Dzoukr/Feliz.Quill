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
        ]
    ]


let contentPart model =
    match model.CurrentPage with
    | Index -> Pages.Index.view


[<ReactComponent>]
let AppView () =
    let model, dispatch = React.useElmish(init, update, [| |])
    let render =
        Bulma.container [
            Bulma.section [
                Bulma.tile [
                    tile.isAncestor
                    prop.children [
                        Bulma.tile [
                            tile.is2
                            prop.children (menuPart model)
                        ]
                        Bulma.tile (contentPart model)
                    ]
                ]
            ]
        ]
    React.router [
        router.onUrlChanged (parseUrl >> UrlChanged >> dispatch)
        router.children render
    ]
