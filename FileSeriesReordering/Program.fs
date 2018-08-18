// return an integer exit code
module Program

open System
open System.IO
open System.Text.RegularExpressions

module Program =
    open System.Linq
    
    let readFilesInDirectory (directory : DirectoryInfo) pattern =
        match directory.Exists with
        | false -> Seq.empty
        | true -> directory.EnumerateFiles(pattern)
    
    let (|Regex|_|) pattern input =
        let m = Regex.Match(input, pattern)
        if m.Success then 
            Some(List.tail [ for g in m.Groups -> g.Value ])
        else None
    
    let matchFirstNumberInString =
        function 
        | Regex @"^(\D*)(\d+)" [ _; result ] -> 
            match Int32.TryParse(result) with
            | (true, int) -> Some(int)
            | _ -> None
        | _ -> None
    
    let zipFilenamesAndOrdinals (files : FileInfo list) : (int * FileInfo) list =
        match files with
        | [] -> []
        | list -> 
            list
            |> List.map (fun x -> matchFirstNumberInString x.Name, x)
            |> List.choose (function 
                   | Some int, y -> Some(int, y)
                   | _ -> None)
            |> List.sortBy (fun (x, y) -> x)
    
    [<EntryPoint>]
    let main argv =
        let currentDirectory = new DirectoryInfo(Environment.CurrentDirectory)
        let fileInfos = readFilesInDirectory currentDirectory "*.*" |> Seq.toList
        let zipped = zipFilenamesAndOrdinals fileInfos
        let (min, _) = List.minBy fst zipped
        zipped |> List.iter (fun (x, name) -> printfn "Found file %i: %s" (x - min + 1) name.Name)
        printfn "All files enumerated"
        0
