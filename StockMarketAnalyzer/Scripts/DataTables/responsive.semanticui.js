/*! Bootstrap integration for DataTables' Responsive
 * ©2015-2016 SpryMedia Ltd - datatables.net/license
 */

(function( factory ){
	if ( typeof define === 'function' && define.amd ) {
		// AMD
		define( ['jquery', 'datatables.net-se', 'datatables.net-responsive'], function ( $ ) {
			return factory( $, window, document );
		} );
	}
	else if ( typeof exports === 'object' ) {
		// CommonJS
		module.exports = function (root, $) {
			if ( ! root ) {
				root = window;
			}

			if ( ! $ || ! $.fn.dataTable ) {
				$ = require('datatables.net-se')(root, $).$;
			}

			if ( ! $.fn.dataTable.Responsive ) {
				require('datatables.net-responsive')(root, $);
			}

			return factory( $, root, root.document );
		};
	}
	else {
		// Browser
		factory( jQuery, window, document );
	}
}(function( $, window, document, undefined ) {
'use strict';
var dataTable = $.fn.dataTable;


var display = dataTable.Responsive.display;
var original = display.modal;
var modal = $(
	'<div class="ui modal" role="dialog">'+
		'<div class="header">'+
			'<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>'+
		'</div>'+
		'<div class="content"/>'+
	'</div>'
);

display.modal = function ( options ) {
	return function ( row, update, render ) {
		if ( ! $.fn.modal ) {
			original( row, update, render );
		}
		else {
			if ( ! update ) {
				if ( options && options.header ) {
					modal.find('div.header')
						.empty()
						.append( '<h4 class="title">'+options.header( row )+'</h4>' );
				}

				modal.find( 'div.content' )
					.empty()
					.append( render() );

				modal
					.appendTo( 'body' )
					.modal('show');
			}
		}
	};
};


return dataTable.Responsive;
}));
