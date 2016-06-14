Param($folder, $testdll)

get-childitem $folder -include App.*.config -recurse | foreach ($_) {
	$shortname = $_.name -replace 'App.', '' -replace '.config', '' -replace '[.]',''
	$newdll = $testdll -replace '[.]dll', ("." + $shortname + ".dll")

	Copy-Item  ($folder + "\" + $testdll) ($folder + "\" + $newdll)
	
	$sourceConfig = ($folder + "\" + $testdll + ".config")
	$transform = $_.fullname
	$targetConfig = ($folder + "\" + $newdll + ".config")
	
	.\ConfigTransform.exe $sourceConfig $transform $targetConfig
}

remove-item ($folder + "\" + $testdll) 
remove-item ($folder + "\" + $testdll + ".config")