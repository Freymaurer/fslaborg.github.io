#r "../_lib/Fornax.Core.dll"
#load "layout.fsx"

open Html

let getProcessedCardBody (card:Cardloader.MainPageCard) =
    card.CardBody
        .Replace("<strong>",(sprintf "<strong class='is-emphasized-one-third-%s'>" card.CardEmphasisColor))
        .Replace("<h3>","<h3 class='main-subtitle'>")

let renderPrimaryCard (card:Cardloader.MainPageCard) =
    div [Class (sprintf "card-is-%s main-Container" card.CardColor)] [
        div [Class "main-TextField is-skewed-right"] [
            h2 [Class (sprintf "main-title has-bg-%s" card.CardEmphasisColor )] [!! card.CardTitle]
            div [Class "container is-centered"] [
                div [Class "image-scroll-container is-centered has-text-centered"] [
                    !! "images will scroll here"
                ]
            ]
            div [Class "main-text"] [
                !! (getProcessedCardBody card)
            ]
        ]
    ]


let renderSecondaryCard isLeft (card:Cardloader.MainPageCard) = 
    if isLeft then 
        div [Class (sprintf "card-is-%s main-Container" card.CardColor)] [
            div [Class "columns"] [
                div [Class "column"] [
                    div [Class "main-TextField is-skewed-left"] [
                        h2 [Class (sprintf "main-title has-bg-%s" card.CardEmphasisColor )] [!! card.CardTitle]
                        div [Class "main-text"] [
                            !! (getProcessedCardBody card)
                        ]
                    ]
                ]
                div [Class "column"] [
                    div [Class "main-ImageContainer"] [
                        a [Href "https://github.com/fslaborg"; Target "_blank"] [
                            figure [Class "image is-square"] [
                                img [Src card.CardImages.[0]]
                            ]
                        ]
                    ]
                ]
            
            ]
        ]
    else
        div [Class (sprintf "card-is-%s main-Container" card.CardColor)] [
            div [Class "columns"] [
                div [Class "column"] [
                    div [Class "main-ImageContainer"] [
                        a [Href "https://github.com/fslaborg"; Target "_blank"] [
                            figure [Class "image is-square"] [
                                img [Src card.CardImages.[0]]
                            ]
                        ]
                    ]
                ]
                div [Class "column"] [
                    div [Class "main-TextField is-skewed-right"] [
                        h2 [Class (sprintf "main-title has-bg-%s" card.CardEmphasisColor )] [!! card.CardTitle]
                        div [Class "main-text"] [
                            !! (getProcessedCardBody card)
                        ]
                    ]
                ]
            
            ]
        ]
let generate' (ctx : SiteContents) (_: string) =
    
    let cards : Cardloader.MainPageCard list= 
        ctx.TryGetValues<Cardloader.MainPageCard>()
        |> Option.defaultValue Seq.empty
        |> Seq.toList

    Layout.layout ctx "Home" [
        section [Class "section"] (
            cards
            |> List.sortBy (fun c -> c.CardIndex)
            |> List.mapi (fun i card ->
                let isLeft = (i+1)%2=0
                match card.CardType with
                | Cardloader.CardType.Main ->
                    renderPrimaryCard card
                | Cardloader.CardType.Secondary ->
                    renderSecondaryCard isLeft card
            )
        )
    ]


let generate (ctx : SiteContents) (projectRoot: string) (page: string) =
  generate' ctx page
  |> Layout.render ctx