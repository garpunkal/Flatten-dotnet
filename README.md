# Flatten

A .NET 8 console tool that flattens a directory tree — moving all files from subdirectories into the root directory and removing the now-empty folders. Optionally removes files by extension after flattening.

## Usage

Run the tool from the directory you want to flatten:

```bash
cd /path/to/directory
Flatten
```

All files in subdirectories are moved to the current directory. If a filename collision occurs, a counter suffix is appended (e.g. `file (1).txt`).

## Configuration

Edit `appsettings.json` to configure which file extensions are removed after flattening:

```json
{
  "FileExtensionsToBeRemoved": ".txt,.nfo,.jpg,.png,.bmp,.md,.doc,.docx,.jpeg,.gif,.exe"
}
```

Leave the value empty to skip the cleanup step.

## Building

```bash
dotnet build
```

## Publishing

```bash
dotnet publish -c Release
```

## License

[MIT](LICENSE)
