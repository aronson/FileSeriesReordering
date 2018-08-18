// return an integer exit code
module Program

open System
open System.IO

module Program =
    let readFilenamesInDirectory (directory : DirectoryInfo) pattern =
        match directory.Exists with
        | false -> Seq.empty
        | true -> directory.EnumerateFiles(pattern) |> Seq.map (fun f -> f.Name)
    
    [<EntryPoint>]
    let main argv =
        let currentDirectory = new DirectoryInfo(Environment.CurrentDirectory)
        readFilenamesInDirectory currentDirectory "*.*" 
        |> Seq.iter (printfn "%s")
        printfn "All files enumerated"
        0
