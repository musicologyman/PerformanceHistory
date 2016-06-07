open System
open System.IO

exception DirectoryNotFound of string

let pwd() = printfn "%s" Environment.CurrentDirectory

let cwd(dirname) =
    if Directory.Exists(dirname) then
        Environment.CurrentDirectory <- dirname
    else
        let message = sprintf "Directory %s does not exist" dirname
        raise (DirectoryNotFound(message))

let ls() =
    Directory.GetFileSystemEntries(".")