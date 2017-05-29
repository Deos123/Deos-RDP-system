# Deos-RDP-system

RDP Management and creator system for GHF.

Components:

"Hidden RDP" AKA Stub = create new user,make admin,hidden,add to local rdp usersgroup,send data of the pc to server. //Public for improvements

"Panel" = Parse data from server and display it,ping each target to see which ones are online. //Private

As 29/05/2017 stub detection rate is 2/37 (Both false positives since it uses the RDP Wrapper library).Will stay "fud" forever since it doesnt involve any kind of malicious code (well,the code isn't malicious by itself,the way I use it is bad :D ).
