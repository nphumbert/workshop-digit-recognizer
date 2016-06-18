open System
open System.IO

type Example = { Label:int; Pixels:int[] }

let parseFile (file: string) =
    File.ReadAllLines file
    |> Array.map(fun s -> s.Split(','))
    |> fun columns -> columns.[ 1 .. ]
    |> Array.map(fun line -> line 
                            |> Array.map(fun s -> (int) s)) 
    |> Array.map(fun line -> { Label = line.[0]; Pixels = line.[ 1 .. ] })

let distance (p1: int[]) (p2: int[]) =
    Array.map2(fun p1 p2 -> (p1 - p2) * (p1 - p2)) p1 p2
    |> Array.sum
    |> float
    |> sqrt

let trainingData = parseFile(__SOURCE_DIRECTORY__ + "/trainingsample.csv")
    
let classify (unknown:int[]) =
    trainingData 
    |> Array.minBy(fun example -> distance unknown example.Pixels)    
    |> fun prediction -> prediction.Label

type ValidatedExample = { RealValue:int; ClassifiedValue:int }  

let validatedData = 
    parseFile(__SOURCE_DIRECTORY__ + "/validationsample.csv") 
    |> Array.map(fun validationData -> { RealValue = validationData.Label; ClassifiedValue = classify validationData.Pixels  })

let correctAnswers = 
    validatedData 
    |> Array.countBy(fun v -> v.RealValue = v.ClassifiedValue) 