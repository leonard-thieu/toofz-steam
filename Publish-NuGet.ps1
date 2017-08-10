$appveyor_repo_tag = $env:APPVEYOR_REPO_TAG

if ($appveyor_repo_tag -eq "true") {
	$configuration = $env:CONFIGURATION
	$platform = $env:PLATFORM
	$appveyor_build_folder = $env:APPVEYOR_BUILD_FOLDER
	$project = $env:PROJECT

	if ($configuration -eq "") {
		$configuration = "Debug";
	}
	switch ($platform) {
		"x86" { break }
		"x64" { break }
		"ARM" { break }
		default { $platform = "AnyCPU" }
	}

	nuget pack "$appveyor_build_folder\$project\$project.csproj" -Properties "Configuration=$configuration;Platform=$platform" -Symbols

	[xml]$nuspec = Get-Content -Path "$appveyor_build_folder\$project\$project.nuspec"
	$version = $nuspec.package.metadata.version

	$api_key = $env:NUGET_API_KEY

	nuget push "$appveyor_build_folder\$project-$version.nupkg" -Source "https://ci.appveyor.com/nuget/leonard-thieu-h8hvbe0oe9o1/api/v2/package" -ApiKey "$api_key" -SymbolSource "https://ci.appveyor.com/nuget/leonard-thieu-h8hvbe0oe9o1/api/v2/package" -SymbolApiKey "$api_key" -NonInteractive
} else {
	Write-Host ""NuGet" deployment has been skipped as environment variable has not matched ("appveyor_repo_tag" is "$appveyor_repo_tag", should be "true")"
}