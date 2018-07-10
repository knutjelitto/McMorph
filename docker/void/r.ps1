$cwd=[string](Get-Location)
$root=$cwd + "/root:/root"
$morph=$cwd + "/../../../McMorph:/root/McMorph"
$pogo="Pogo:/root/Pogo"
docker run --rm -it --privileged --volume $root --volume $morph --volume $pogo void
#docker run --rm -it --privileged void