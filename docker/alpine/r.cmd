@docker run --hostname pogo-alpine --privileged -v %cd%\root:/root -v %cd%\..\..\..\LiFo:/root/LiFo -v %cd%\..\..\..\McMorph:/root/McMorph -v Pogo:/Pogo -it alpine sh -l
