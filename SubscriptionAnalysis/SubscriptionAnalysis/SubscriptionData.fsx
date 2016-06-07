#r """.\packages\FSharp.Data.2.3.0\lib\net40\FSharp.Data.dll"""
#r "System.Xml.Linq.dll"
open FSharp.Data

[<Literal>]
let filename = """Y:\Repos\PerformanceHistory\Programs\1963-64_TO_1973-74.subscriptions.xml"""

type Subscriptions = XmlProvider<filename, InferTypesFromValues=false>

let sample = Subscriptions.GetSample()

let programs = sample.Programs

programs
|> Seq.length