
$directory = "target/gatling"
$lastRun = Join-Path $directory "lastRun.txt"
$lines = get-content $lastRun

$output = "<html>"
$output += "<head>"
$output += "<title>Gattling Reports Summary</title>"
$output += '<link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet" integrity="sha384-T3c6CoIi6uLrA9TneNEoa7RxnatzjcDSCmG1MXxSR1GAsXEV/Dwwykc2MPK8M2HN" crossorigin="anonymous">'
$output += "</head>"
$output += "<body>"
$output += "<div class=""container"">"
$output += "<h1>Gatling Reports Summary</h1>"


foreach($line in $lines){
    $relativePath = Join-Path (Join-Path $directory $line) "index.html"
    
    $reportName = ($line -split '-')[0]
    
    ((get-content -Path $relativePath -raw) -match $reportName) | Out-Null
    
    $camelCaseName = $matches[0]
    $name = ""
    foreach($c in $camelCaseName.ToCharArray()){
        if($c -cmatch "[A-Z]"){
            $name += " ";
        }
        $name += $c
    }
    $name = $name.Trim()

    $absolutePath = $relativePath | Resolve-Path
    $output += "<h3>$name (<a href=""$absolutePath"">Open</a>)<h3>"
    $output += '<div class="embed-responsive embed-responsive-16by9">'
    $output += '<iframe style="height:60vh; width:100%" class="embed-responsive-item" src="' + $absolutePath + '"></iframe>'
    $output += '</div>'
}

$output += '<script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/js/bootstrap.bundle.min.js" integrity="sha384-C6RzsynM9kWDrMNeT87bh95OGNyZPhcTNXj1NW7RuBCsyN/o0jlpcV8Qyq46cDfL" crossorigin="anonymous"></script>'
$output += "</div>"
$output += "</body>"
$output += "</html>"

$outputPath = (Join-Path $directory "summary.html")
$output | set-content -Path $outputPath -Force

Start-Process $outputPath


