# Unity.Inspector

A Unity package that provides custom inspector attributes to enhance the Unity Inspector experience with read-only field support.

## Installation

1. Installation via UPM with git url
2. Copy to Assets Folder

## Overview

This package extends Unity's Inspector system with custom attributes that allow developers to create read-only fields in the Inspector. This is useful for displaying calculated values, debug information, or any data that should be visible but not editable in the Inspector.

## Features

- **ReadOnly Attribute**: Makes fields visible but non-editable in the Unity Inspector
- **Custom Property Drawer**: Provides seamless integration with Unity's Inspector system
- **Simple Implementation**: Easy to use with minimal code overhead
- **Type Safety**: Works with all Unity serializable types

## Components

### ReadOnlyAttribute
A custom attribute that marks fields as read-only in the Unity Inspector:
- Inherits from `PropertyAttribute`
- Located in `medzumi.Attributes` namespace
- Works with any serializable field type

### ReadOnlyDrawer
A custom property drawer that handles the visual representation:
- Implements `PropertyDrawer` for `ReadOnlyAttribute`
- Disables GUI controls while maintaining field visibility
- Located in `medzumi.Attributes.Editor` namespace

## TODO List

- Additional inspector attributes and enhanced functionality