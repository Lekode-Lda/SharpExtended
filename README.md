# SharpExtended

A collection of extensions and utilities for C# useful for many projects.

* **Author:** Pedro Cavaleiro
* **Contact:** p.cavaleiro@lekode.com

## Table of Contents
* [Extended Types](#extended-types)
* [Utilities](#utilities)
* [Support](#support)
* [Roadmap](#roadmap)
* [Contribute](#contributing)
* [Change Log](#change-log)
* [License](#license)

## Extended Types

* [Array](#array)
* [DateTime](#datetime)
* [Enum](#httpclient)
* [HttpClient](#httpclient)
* [HttpResponseMessage](#httpresponsemessage)
* [IEnumerable](#ienumerable)
* [Int](#int)
* [JsonSerializer](#jsonserializer)
* [List](#list)
* [NameValueCollection](#namevaluecollection)
* [Object](#object)
* [String](#string)

### Array
#### RemoveIndices
Allows to remove a specific index of a array.
* Signature: `public static T[] RemoveIndices<T>(this T[] indicesArray, int removeAt)`

#### AddElementToArray
Adds a element to the end of a array.
* Signature: `public static T[] AddElementToArray<T>(this T[] array, T element)`

#### TruncateStrings
Truncates all strings in a array to a specific size.
* Signature: `public static string[] TruncateStrings(this string[] arr, int maxLength)`

#### ToIntArray
Converts a string array to a int array. A non int value will throw a exception.
* Signature: `public static int[] ToIntArray(this string[] arr)`

#### TrimStrings
Trims all strings in a array.
* Signature: `public static string[] TrimStrings(this string[] arr)`

#### ToStringArray
Converts a array of enums to a array of strings, the enums must contain the `[Description(""")]` attribute (see [Enum](#enum)).
* Signature: `public static string?[] ToStringArray<T>(this T[] enumerator)`

#### ToList
Converts a array of strings to a list of the specified enum. Strings that can't be parsed will be ignored.
* Signature: `public static List<T> ToList<T>(IEnumerable<string> arr) where T : Enum`

### DateTime
#### TruncateDate
Truncates a DateTime to a specific time component.

Available Components:
* Year
* Quarter
* Month
* Week
* Day
* Hour
* Minute
* Second

Signature: `public static DateTime TruncateDate(this DateTime inDate, Accuracy accuracy)`

#### AddsWeeks
Adds weeks to the DateTime.
* Signature: `public static DateTime AddWeeks(this DateTime date, int value)`

#### InWeek
Verifies if a date is in the current week.
* Signature: `public static bool InWeek(DateTime date)`

#### InMonth
Verifies if a date is in the current month.
* Signature: `public static bool InMonth(DateTime date)`

#### CurrentWeek
Gets the current week, returns the start and end of the week.
* Signature: `public static (DateTime, DateTime) CurrentWeek()`

#### CurrentMonth
Gets the current month, returns the start and end of the month.
* Signature: `public static (DateTime, DateTime) CurrentMonth()`

### Enum

For these extensions the enum must contain the attribute `[Description("")]`.

Example
```csharp
public enum SomeEnum {
    [Description("enumvalue1")]
    EnumValue1
    //...
}
```

#### ParseEnum
Parses a string to the specified enum. If it's not possible to parse the enum the exception `InvalidEnumException` will be thrown.
* Signature: `public static T ParseEnum<T>(this string str) where T : Enum`

#### TryParseEnum
Attempts to parse a string to the specified enum. Returns true if it's possible to parse false otherwise, use the parameter `@enum` to get the enum value.
* Signature: `public static bool TryParseEnum<T>(this string str, out T? @enum) where T : Enum`

#### GetDescription
Gets the description of a enumerator.
* Signature: `public static string? GetDescription(this Enum? e)`

### HttpClient
#### PatchAsJsonAsync
Allows to send a http `Patch` request with Json content.
* Signature: `public static Task<HttpResponseMessage> PatchAsJsonAsync<T>(this HttpClient client, string requestUri, T value)`

### HttpResponseMessage
#### ReadJsonContentAsAsync
Allows you directly read from the HttpResponseMessage into a model matching the response Json.
* Signature:
  ```csharp
  public static async Task<T?> ReadJsonContentAsAsync<T>(
    this HttpContent       content,
    JsonSerializerOptions? options = null
  )
  ```

### IEnumerable
#### Each
Iterates through a IEnumerable also providing the current index. **Note:** Does not support break .
* Signature: `public static void Each<T>(this IEnumerable<T> ie, Action<T, int> action)`

### Int
#### CryptoRandom
Generates a random number cryptographically within a range.
* Signature: `public static int CryptoRandom(int min = 0, int max = 10)`

### JsonSerializer
#### DeserializeAnonymousType
Allows you to deserialize a Json to an anonymous type effortlessly.
* Signature: `public static T? DeserializeAnonymousType<T>(this string json, T anonymousTypeObject, JsonSerializerOptions? options = default)`

#### Deserialize
Deserializes a string to a model matching the Json.
* Signature: `public static T? Deserialize<T>(this string json, JsonSerializerOptions? options = default)`

#### Serialize
Serializes a model.
* Signature: `public static string Serialize<T>(this T json, JsonSerializerOptions? options = default)`

### List
#### StringsToLowerCase
Transforms all strings in list to lowercased.
* Signature: `public static List<string> StringsToLowerCase(this IEnumerable<string> list)`

#### ScrambledEquals
Checks if two lists are equal (contains the same elements) ignoring the order.
* Signature: `public static bool ScrambledEquals<T>(IEnumerable<T> list1, IEnumerable<T> list2) where T : notnull`

#### TrimStrings
Trims all strings in a list.
* Signature: `public static List<string> TrimStrings(this IEnumerable<string> list)`

### NameValueCollection
#### ToDictionary
Converts a NameValueCollection to a Dictionary.
* Signature: `public static Dictionary<string, string?> ToDictionary(this NameValueCollection collection)`

### Object
Copies data between two instances of the same class. It's possible to ignore the property marked with `[Key]`.
* Signature: `public static void CopyTo<T>(this T from, T to, bool ignoreKey = true)`

### String
#### ToBool
Converts a string to a boolean. If the string is to a valid boolean it will always return false.
* Signature: `public static bool ToBool(this string value) `

#### Base64Decode
Decodes a base64 string to a string.
* Signature: `public static string Base64Decode(this string base64EncodedData)`

#### ToGuid
Converts a string to a Guid. If it's not possible to parse to a guid it will return null.
* Signature: `public static Guid? ToGuid(this string str)`

#### ToUlong
Converts a string to ulong. If it's not possible to convert it throws a exception.
* Signature: `public static ulong ToUlong(this string str)`

#### ToLong
Converts a string to ulong. If it's not possible to convert it throws a exception.
* Signature: `public static long ToLong(this string str)`

#### ToInt32
Converts a string to int. If it's not possible to convert it throws a exception.
* Signature: `public static int ToInt32(this string str)`

#### ToBase64
Converts a string to a base64 string.
* Signature: `public static string ToBase64(this string plainText)`

#### Truncate
Truncates a string to the specified maximum length.
* Signature: `public static string Truncate(this string value, int maxLength)`

#### Sha1
Generates the SHA1 hash of the string, the hash is returned in a hex string.
* Signature: `public static string Sha1(this string input)`

#### Sha256
Generates the SHA256 hash of the string, the hash is returned in a hex string.
* Signature: `public static string Sha256(this string text)`

#### HmacSha256
Generates the HMAC-SHA256 of a string, the HMAC is returned in a hex string.
* Signature: `public static string HmacSha256(this string text, string key)`

#### HmacMd5
Generates the HMAC-MD5 of a string, the HMAC is returned in a hex string.

**WARNING:** MD5 it is a weak hashing algorithm and it is highly discouraged it's use .
* Signature: `public static string HmacMd5(this string text, string key)`

#### SaltAndHash
Salts and hashes a string, recommended to secure store passwords. See [Security](#security) on how to securely generate a salt.
* Signature: `public static string SaltAndHash(this string password, string salt) `
* Signature: `public static string SaltAndHash(this string password, byte[] salt)`

#### Md5
Generates the MD5 hash of the string, the hash is returned in a hex string.

**WARNING:** MD5 it is a weak hashing algorithm and it is highly discouraged it's use.
* Signature: `public static string Md5(this string input)`

#### ContainsUnicodeCharacter
Checks if a string contains any unicode characters.
* Signature: `public static bool ContainsUnicodeCharacter(this string input)`

#### IsValidEmail
Checks if a email is valid.
* Signature: `public static bool IsValidEmail(this string str)`

#### ComparePasswords
Compares two passwords, the plain-text password and the stored hash, requires the salt to correctly compare the passwords.
* Signature: `public static bool ComparePasswords(this string password, string storedHash, string salt)`

#### ClosestMatch
Given a list of strings and the input string it returns which string from the list is the closest match to the input. This uses the Levenshtein Distance algorithm.
* Signature: `public static string ClosestMatch(this string text, string[] strings)`

#### GenerateCeo
Based on a string generates a CEO friendly string.
* Signature: `public static string GenerateCeo(this string s)`

#### IsNullOrEmpty
Checks if the string is null or empty.
* Signature: `public static bool IsNullOrEmpty(this string? str)`

#### TruncateHtmlByWords
Truncates a chunk of HTML by the Inner Text of the document but keeps the HTML tags. Unclosed tags will be automatically closed.
* Signature: `public static string TruncateHtmlByWords(this string, int charLimit, bool insertDots = true)`

#### TruncateWords
Truncates text to a maximum amount of characters, this will not split words.
* Signature: `public static string TruncateWords(this string text, int maxLength, out bool truncated)`

## Utilities

* [Json Converters](#json-converters)
  * EnumDescriptionConverter
  * UlongStringifier
* [AesCrypt](#aescrypt)
* [Security](#security)
* [Result](#result)

### Json Converters

The following JsonConverts are available:
* EnumDescriptionConverter;
* UlongStringifier
* LongStringifier

The `EnumDescriptionConverter` converts between the given enum but the enum requires the attribute `[Description("")]`, see [Enum](#enum).

The `UlongStringifier` converts between ulong and string to avoid issues with JavaScript.

The `LongStringifier` converts between long and string to avoid issues with JavaScript. Recommended when using Snowflakes as a correctly implemented Snowflake takes 63bits not requiring a ulong.

### AesCrypt

Allows the encryption and decryption of strings using AES algorithm.

The passphrase must be 128 (16 characters), 192 (24 characters) or 256 (32 characters) bit long and the IV must be 128 bit long (16 characters).

```csharp
var cypher = new AesCrypt(passphrase, iv);
var encrypted = cypher.Encrypt(plaintext);
var decrypted = cypher.Decrypt(encrypted);
```

The following options are also available
```csharp
public AesCryptOptions() {
      PasswordHashSalt       = "";
      PasswordHash           = AesPasswordHash.Sha1;
      PasswordHashIterations = 1;
      MinSaltLength          = 0;
      MaxSaltLength          = 0;
      FixedKeySize           = null; // if null it will be calculated from the key size
      PaddingMode            = PaddingMode.PKCS7;
}
```

### Security

Some of these methods are used by the [extensions](#extended-types) for more convenient use.

#### GenerateSaltedHash
Salts a byte array.
* Signature: `public static byte[] GenerateSaltedHash(byte[] plainText, byte[] salt)`

#### CompareByteArrays
Compares the byte arrays.
* Signature: `public static bool CompareByteArrays(byte[] array1, byte[] array2)`

#### GetSalt
Generates a cryptographically secure salt to a maximum length.
* Signature: `public static byte[] GetSalt(int maximumSaltLength = 64)`

#### Sha1
Generates the Sha1 of a string returning the hash as hex string.
* Signature: `public static string Sha1(string input)`

#### Sha256
Generates the Sha256 of a string returning the hash as hex string.
* Signature: `public static string Sha256(string input)`

#### HmacSha256
Generates the HMAC-SHA256 of a byte array.
* Signature: `public static IEnumerable<byte> HmacSha256(byte[] data, string key)`

#### HmacMd5
Generates the HMAC-MD5 of a byte array.

**WARNING:** MD5 it is a weak hashing algorithm and it is highly discouraged it's use.
* Signature: `public static IEnumerable<byte> HmacMd5(byte[] data, string key)`

#### GenerateRsaKeyPair
Generates an RSA Public/Private key pair.
* Signature: `public static RsaPublicPrivateKeyPair GenerateRsaKeyPair(int size = 2048)`

#### RsaEncryptWithPublic
Encrypts a string using an RSA public key.
* Signature: `public static string RsaEncryptWithPublic(string clearText, string publicKey)`

#### RsaEncryptWithPrivate
Encrypts a string using an RSA private key.
* Signature: `public static string RsaEncryptWithPrivate(string clearText, string privateKey)`

#### RsaDecryptWithPrivate
Decrypts a string using an RSA private key, the input is a base64 string.
* Signature: `public static string RsaDecryptWithPrivate(string base64Input, string privateKey)`

#### RsaDecryptWithPublic
Decrypts a string using an RSA public key, the input is a a base64 string.
* Signature: `public static string RsaDecryptWithPublic(string base64Input, string publicKey)`

#### Md5
Generates a MD5 hash.

**WARNING:** MD5 it is a weak hashing algorithm and it is highly discouraged it's use.
* Signature: `public static byte[] Md5(byte[] data)`

### Result

A small class to use return based error handling. It's usage is rather simple.

The base class `Result` implements the following:

* Properties
  * `bool Success`
  * `string Error`
  * `List<string> StackTrace`
  * `bool IsFailure`

* Methods
  * `Fail("message")`: Returns the Result instance with a error message;
  * `AddFail("new error")`: Adds to the currents result a new error, the current one is pushed to the top of the StackTrace;
  * `Ok()`: Returns the Result instance with no error.

The class `Result` can be expanded and already implemented is `Result<T>` which allows the class to hold a value, this adds the following capabilities:

* Properties:
  * `T Value`

* Methods:
  * `Fail<T>("message")`: Returns the Result instance with a error message, the type `T` is required to properly compute the default type for the `Value` property;
  * `Ok<T>(T value)`: Returns the Result instance with no error and with a `Value` of type `T`.

Usage Examples:
```csharp
var result1 = Result.Fail("some error ocurred");
var result2 = Result.Ok();
var result3 = Result.Fail<string>("a weird error happened");
result3.AddFail("another error ocurred");
var result4 = Result.Ok(complexObject); //C# will infer the type from the parameter
```

It's possible to further expanding from the `Result` and `Result<T>` to add more functionality.

## Support

Use the [integrated issue tracker](https://git.lekode.com/lekode/sharpextended/-/issues).

## Roadmap

The development is done according to new necessities. Hence, no roadmap is currently undefined.

## Contributing

If you have a suggestion that would make this better, please fork the repo and create a pull request. You can also simply open an issue with the tag "enhancement".

1. Fork the project
2. Create your feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push the commit to the branch (`git push origin feature/AmazingFeature`)
5. Open a pull request

## Change Log

[Change Log](CHANGELOG.md)

## License

[MIT License](LICENSE)