# certbot.dotnet.solution
dotnet certbot console manual-auth-hook application for godaddy dns API

This is mainly a project I created to:
1) play around with the GoDaddy REST API
2) play around with .net core 3.1 and deploy to linux-x64
3) create a console application that could be used with certbot to get a letsencrypt wildcard certificate automatically

I know that most of this could be replaced with a simple "curl" script in bash, but I wanted to try and do it this way

To use it:
- build the output (I used Visual Studio, but you can use anything)
- call "dotnet publish -c release -r linux-x64" (I'm deploying to Redhat/CentOs8)
- push it to the server you want to run certbot on
- run this in the folder you deployed to: sudo chmod +x certbot.console
- Add your Godaddy AuthorizationKey to appsettings.json
- run your certbot command, something like this:
certbot certonly --manual --preferred-challenges=dns \
	--email :youremailaddress: \
	--server https://acme-v02.api.letsencrypt.org/directory \
	--agree-tos -d "*.:yourdomainname:" \
	--manual-auth-hook /pathtoyourdeployment/certbot.console \
	--manual-cleanup-hook /pathtoyourdeployment/certbot.console
