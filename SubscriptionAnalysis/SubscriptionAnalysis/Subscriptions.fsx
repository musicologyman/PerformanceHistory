#r "System.Xml.dll" 
#r "System.Xml.Linq.dll" 

open System
open System.Xml
open System.Xml.Linq
open System.Xml.XPath

fsi.ShowProperties <- false
fsi.ShowIEnumerable <- false
fsi.ShowDeclarationValues <- false

type Season = { Start : int; End : int } 
    with
    static member parse ( s : string ) =
        let start = s.Substring(0, 4) |> Int32.Parse 
        let century = start - (start % 100)
        let end' = (s.Substring(5) |> Int32.Parse) + century
        
        if (start % 100 = 99) then
            { Start = start; End = end' + 100 }        
        else
            { Start = start; End = end' }

let X : string -> XName = XName.Get
let (!) : XElement -> string = function e -> e.Value

type XElement 
    with
    member x.Children ( s : string ) = x.Elements ( X s )
    member x.Descendants ( s : string ) = x.Descendants ( X s )

let filename = "../Programs/complete.xml"

let doc = XDocument.Load(filename)
let root = doc.Root

let programs = 
    root.XPathSelectElements("program[.//eventType='Subscription Season']")
    
let composers =
    programs.Descendants(XName.Get "composerName")
    |> Seq.map (!)
    |> Seq.groupBy (fun s -> s) 
    |> Seq.map (fun (name, values) -> (name, values |> Seq.length)) 
    |> Seq.sortByDescending snd 

let seasons = 
    root.XPathSelectElements(".//season")
    |> Seq.map (!)
    |> Seq.map Season.parse
    |> Seq.distinct

let findComposer s =
    root.XPathSelectElements(sprintf "program[contains(.//composerName, '%s')]" s)

let findComposerAndConductor composer conductor =
    root.XPathSelectElements(
        sprintf "program[contains(.//composerName, '%s') and contains(.//conductorName, '%s')]" composer conductor)

let eventTypes = 
    root.XPathSelectElements(".//eventType") 
    |> Seq.map (!) 
    |> Seq.distinct

let mahler = 
    root.XPathSelectElements("program[contains(.//composerName, 'Mahler')]")

let mahler_bernstein = 
    root.XPathSelectElements("program[contains(.//composerName, 'Mahler') and contains(.//conductorName, 'Bernstein')]");

let mahler_boulez = 
    root.XPathSelectElements("program[contains(.//composerName, 'Mahler') and contains(.//conductorName, 'Boulez')]");
