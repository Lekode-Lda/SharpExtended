# Change Log

This file contains the change log for all releases

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
