# spike-crytpo-sign
Spike for crytpo signing in dotnet standard


### creasting the needed files

(utilize git bash)

``` bash
mkdir -p /c/code/certs/cryptosign/
cd /c/code/certs/cryptosign/

openssl req \
       -newkey rsa:4096 -nodes -keyout private.key \
       -x509 -days 365 -out public.crt

openssl pkcs12 -in public.crt -inkey private.key -export -out fullkey.pfx -passout pass:


```
### running
``` bash
dotnet Spike.CryptoSign.Cli.dll Test  -Text "coding is fun" -FullKey C:/code/certs/cryptosign/fullkey.pfx -PublicKey C:/code/certs/cryptosign/public.crt
```
