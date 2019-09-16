/**
 * @license Copyright (c) 2003-2019, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see https://ckeditor.com/legal/ckeditor-oss-license
 */

CKEDITOR.editorConfig = function( config ) {
	// Define changes to default configuration here. For example:
	// config.language = 'fr';
	// config.uiColor = '#AADC6E';

	config.syntaxhighlight_lang = 'csharp';
	config.syntaxhighlight_hideControls = true;
	config.languages = 'vi';
	config.filebrowserBrowseUrl = '/ckfinder/ckfinder.html';
	config.filebrowserImageBrowseUrl = '/ckfinder/ckfinder.html?Types=Images';
	config.filebrowserFlashBrowseUrl = '/ckfinder/ckfinder.html?Types=Flash';
	config.filebrowserUploadUrl = '/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=File';
	config.filebrowserImageUploadUrl = '/Images/UploadPics';
	config.filebrowserFlashUploadUrl = '/ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash';

	CKFinder.setupCKEditor(null, '/ckfinder/');
};
