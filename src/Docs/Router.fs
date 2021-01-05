module Docs.Router

open Feliz.Router

type Page =
    | Index

let defaultPage = Index

let parseUrl = function
    | _ -> Index

let getHref = function
    | Index -> Router.format("")
