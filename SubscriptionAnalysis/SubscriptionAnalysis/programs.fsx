#r "System.Xml.dll"
#r "System.Xml.Linq.dll"

open System
open System.Xml
open System.Xml.Linq
open System.Xml.XPath

fsi.ShowDeclarationValues <- false
fsi.ShowProperties <- false

let filename = "../Programs/complete.xml"

let doc = XDocument.Load(filename)
let root = doc.Root

let subscription_programs = 
    root.XPathSelectElements("program[//eventType='Subscription Season']")
    
let subscription_composers =
    subscription_programs.Descendants(XName.Get "composerName")
    |> Seq.map (fun e -> e.Value)
    |> Seq.groupBy (fun s -> s) 
    |> Seq.map (fun (name, values) -> (name, values |> Seq.length)) 
    |> Seq.sortByDescending snd 


