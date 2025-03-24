# Adding New Categories to Moodle User Data Export

This documentation explains how to add a new category with test data to the Moodle User Data Export.

## Overview

The Moodle User Data Export is a structured collection of HTML, CSS, and JavaScript files that display exported user data in a navigable format. The export is organized in a tree-like structure with categories and subcategories, each containing data files.

## Required Files and Structure

To add a new category, you'll need to create:

1. A directory structure for the category and its subcategories
2. Data files in JavaScript (.js) and JSON (.json) format
3. References to these files in the data index
4. Navigation entries in the HTML interface

## Step-by-Step Guide

### 1. Create Directory Structure

Create a directory for your new category in the `System _.1` folder. The naming convention typically includes a unique identifier suffix (e.g., `_.20`).

```bash
mkdir -p "/path/to/export/System _.1/Your Category _.XX/Your Subcategory"
```

### 2. Create Data Files

For each category and subcategory, you need to create two data files:

#### Category Data Files

**data.js**

```javascript
var data_file_XX = {
  name: "Your Category",
  description: "Description of your category",
  items: [
    {
      id: 1,
      name: "Item 1",
      value: "Value 1",
    },
    {
      id: 2,
      name: "Item 2",
      value: "Value 2",
    },
  ],
};
```

**data.json**

```json
{
  "name": "Your Category",
  "description": "Description of your category",
  "items": [
    {
      "id": 1,
      "name": "Item 1",
      "value": "Value 1"
    },
    {
      "id": 2,
      "name": "Item 2",
      "value": "Value 2"
    }
  ]
}
```

#### Subcategory Data Files

Create similar files in the subcategory folder with appropriate data.

### 3. Update the Data Index

Edit the `js/data_index.js` file to include references to your new data files:

```javascript
var user_data_index = {
  // ...existing entries...
  data_file_XX: "System _.1/Your Category _.XX/data.js",
  data_file_YY: "System _.1/Your Category _.XX/Your Subcategory/data.js",
};
```

Use unique identifiers for `data_file_XX` that don't conflict with existing entries. Typically, you should use the next available numbers.

### 4. Update the Navigation Tree

Edit the `index.html` file to add your new category to the navigation tree. Look for the `<ul class="treeview parent block_tree list" id="my-tree">` section and add your category in the appropriate location:

```html
<li class="menu-item" role="treeitem" aria-expanded="false">
  Your Category
  <ul class="parent" role="group">
    <li class="menu-item" role="treeitem" aria-expanded="false">
      Your Subcategory
      <ul class="parent" role="group">
        <li class="item" role="treeitem" aria-expanded="false">
          <a data-var="data_file_YY" href="#">data.json</a>
        </li>
      </ul>
    </li>
    <li class="item" role="treeitem" aria-expanded="false">
      <a data-var="data_file_XX" href="#">data.json</a>
    </li>
  </ul>
</li>
```

Make sure the `data-var` attributes match the identifiers you used in the data index.

## Example

### Directory Structure

```
System _.1/
  └── Test Category _.20/
      ├── data.js
      ├── data.json
      └── Test Subcategory/
          ├── data.js
          └── data.json
```

### Data Files Content

**Test Category data.js**

```javascript
var data_file_28 = {
  name: "Test Category",
  description: "This is a test category created on March 24, 2025",
  items: [
    {
      id: 1,
      name: "Test Item 1",
      value: "Sample value 1",
    },
    {
      id: 2,
      name: "Test Item 2",
      value: "Sample value 2",
    },
  ],
};
```

**Test Subcategory data.js**

```javascript
var data_file_29 = {
  name: "Test Subcategory",
  description: "This is a test subcategory with sample data",
  test_data: {
    string_value: "Sample string",
    number_value: 42,
    boolean_value: true,
    array_value: [1, 2, 3, 4, 5],
    nested_object: {
      key1: "value1",
      key2: "value2",
    },
  },
};
```

### data_index.js Update

```javascript
var user_data_index = {
  // ...existing entries...
  data_file_28: "System _.1/Test Category _.20/data.js",
  data_file_29: "System _.1/Test Category _.20/Test Subcategory/data.js",
};
```

### Navigation Tree Update

```html
<li class="menu-item" role="treeitem" aria-expanded="false">
  Test Category
  <ul class="parent" role="group">
    <li class="menu-item" role="treeitem" aria-expanded="false">
      Test Subcategory
      <ul class="parent" role="group">
        <li class="item" role="treeitem" aria-expanded="false">
          <a data-var="data_file_29" href="#">data.json</a>
        </li>
      </ul>
    </li>
    <li class="item" role="treeitem" aria-expanded="false">
      <a data-var="data_file_28" href="#">data.json</a>
    </li>
  </ul>
</li>
```

## Best Practices

1. **Unique Identifiers**: Ensure all data file identifiers are unique to avoid conflicts
2. **Consistent Naming**: Follow the existing naming convention with underscore and number suffix
3. **Complete Data Pair**: Always create both .js and .json files for each category
4. **Data Structure**: Match the data structure to the expected format in the viewer
5. **Testing**: After adding, open the export in a web browser to verify the new category appears correctly

## Troubleshooting

- If the category doesn't appear, check the HTML navigation tree structure
- If the data doesn't load, verify the paths in data_index.js are correct
- Ensure JavaScript variable names in .js files match the references in data_index.js
- Check for syntax errors in your JSON and JavaScript files
