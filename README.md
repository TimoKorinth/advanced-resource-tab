# Advanced Resource Tab
### for Blend for Visual Studio 2012 and 2013
Advanced Resource Tab is an extension to Blend for Visual Studio which adds an additional tab window to get more control over the default resource tab.

Advanced Resource Tab is an extension to Blend for Visual Studio 2012 / 2013 which adds an additional tab window to get more control over the default resource tab.

By default the resource dictionaries in the resource tab of Blend for Visual Studio (previously Expression Blend) aren't in any useful order. If you got a large number of resources in your solution this is a real pain and time killer, searching for the resources you want. As long as I can imagine, this is fact in Expression Blend.

To get around this annoying bug, I've created the Advanced Resource Tab for Blend.

## Features
- Sorting resource dictionaries in the resource tab (ascending / descending)
- Provide buttons to expand / collapse all resource dictionaries
- Quick Searching / Filtering of resources in the resource tab
- Include (sub-)resources in searching / filtering
- Enhances the default resource tab window
- Regular Blend extension
- Shortcut to focus the search-field

![AdvancedResourceTab](http://timokorinth.de/wp-content/uploads/2013/07/AdvancedResourceTab.png)

## Installation

There are two versions tagged: one for Blend 2012 and another for Blend 2013.

###Blend 2012
All you have to do is to copy the AdvancedResourceTab.Extension.dll into the extension folder of Blend. Typically this is "C:\Program Files (x86)\Microsoft Visual Studio 11.0\Blend\Extensions".

###Blend 2013
All you have to do is to copy the AdvancedResourceTab.Extension.dll and *.addin into the extension folder of Blend. Typically this is "C:\Program Files (x86)\Microsoft Visual Studio 12.0\Blend\Addins".

## Usage

Now, if you start Blend, by default the resource dictionaries in the resource tab are in an alphabetically order (ascending).

But if you want more control over the default resource tab, you have to activate the Advanced Resource Tab window:

![AdvancedResourceTabWindow](http://timokorinth.de/wp-content/uploads/2013/07/AdvancedResourceTabWindow.png)

After activating the Advanced Resource Tab window, you can move / dock it in Blend. Make sure to dock it on top of the existing resource tab, so you can see both at a time. Before dropping it, a white layer is visible to indicate that it’s going on top of the tabs:

![AdvancedResourceTabDocking](http://timokorinth.de/wp-content/uploads/2013/07/AdvancedResourceTabDocking.png)

Now you have full control over the resource tab in Blend. Just change the ordering or type in a resource dictionary name to get live result filtering in the resource tab.
