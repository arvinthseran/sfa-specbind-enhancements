param($installPath, $toolsPath, $package, $project)
# Sets the configuration files to have dependent transforms (.Debug/.Release).
# Selections of items in the project are done with Where-Object rather than direct
# access into the ProjectItems collection because if the object is moved or doesn't
# exist then Where-Object will give us a null response rather than the error that
# DTE will give us.

$baseConfig = $project.ProjectItems | Where-Object { $_.Properties.Item("Filename").Value -eq "App.config" -and $_.ProjectItems.Count -eq 0 }
if($baseConfig -eq $null)
{
  # Upgrade scenario - user has moved/removed the MyConfig.config file
  # or it already has the dependent items set.
  return
}

# Config file exists, so update the properties.
$baseConfig.Properties.Item("SubType").Value = "Designer"

$debugConfigs = $project.ProjectItems | Where-Object { $_.Properties.Item("Filename").Value -match "^App[.].*[.]config" }

$debugConfigs | ForEach-Object -process {
	# Handle the update for MyConfig.Debug.config - set it as BuildAction = None
	# and move it to be a dependency of MyConfig.config.
	$_.Properties.Item("ItemType").Value = "None"
	$baseConfig.ProjectItems.AddFromFile($_.Properties.Item("FullPath").Value)
}