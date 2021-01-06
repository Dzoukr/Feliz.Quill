module Docs.Router

open Feliz.Router

type Page =
    | Index
    | Installation
    | QuickStart
    | Toolbars
    | Themes

let defaultPage = Index

let parseUrl = function
    | [ "quickstart" ] -> QuickStart
    | [ "installation" ] -> Installation
    | [ "toolbars" ] -> Toolbars
    | [ "themes" ] -> Themes
    | _ -> Index

let getHref = function
    | Index -> Router.format("")
    | Installation -> Router.format("installation")
    | QuickStart -> Router.format("quickstart")
    | Toolbars -> Router.format("toolbars")
    | Themes -> Router.format("themes")
