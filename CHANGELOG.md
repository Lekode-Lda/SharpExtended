# Change Log

This file contains the change log for all releases

## 1.5.0

* Targets .NET 9 
* Addresses `Package 'System.Text.Encodings.Web' 4.5.0 has a known critical severity vulnerability` warning 

## 1.4.1

* Fixes issue with the JSON EnumDescriptionConverter where it wasn't possible to parse using optional Enums

## 1.4.0

* `imgUrl.DownloadImage(string path)`: Download Image to Disk
* `imgUrl.DownloadImage()`: Download Image to Memory

## 1.3.1

* Prevents a crash when trying to close the stack of tags on TruncateHtmlByWords

## 1.3.0

* Removed and moved RSA methods, the following methods are now available under the `SharpExtended.Rsa` namespace
  * GenerateKeys
  * Encrypt
  * Decrypt
* Simplified and updated AES methods, AES no longer requires initialization and can be called like this
  * `Aes.Encrypt(plaintext, key, iv)`
  * `Aes.Decrypt(cyphertext, key, iv)`

The intellisense documentation was updated for all these methods.

## 1.2.0

* Adds LongStringifier json converter

## 1.1.0

* Adds string extension `TruncateWords`
* Adds string extension `TruncateHtmlByWords`
* Adds string extension `ToLong`
* Compresses string extension `ToBool`

## 1.0.0

* Initial Release
