Caliburn.Micro.Reactive.Extensions
==================================

### Rx based extensions for Caliburn Micro - [![cogenity MyGet Build Status](https://www.myget.org/BuildSource/Badge/cogenity?identifier=ca24b530-5018-4566-8d57-05b62ef15b08)](https://www.myget.org/)

A few constructs that allow the developer to write less code, in a more declarative manner resulting in less bugs and better performance. The classes are similar in nature to those found in ReactiveUI but are simpler (relying on the developer composing higher-order functionality rather than trying to build it in) and tied to Caliburn Micro (mostly as it is my MVVM framework of choice and provides a convenient extension interfaces - namely INotifyPropertyChangedEx).

## Classes

### ObservableProperty

ObservableProperties are much like dependency properties. They are declared in place of a backing field, are exposed as properties and automatically handle property change notifications. On top of this, ObservableProperty instances are generic (removing the awful casting you need with DependencyProperty getters) and expose IObservable<T> of values which can be subscribed to in order to compose functionality in the UI.

### ObservableCommand

### ObservableEvent

## Packages And Symbols

Nuget package available from [our nuget feed](https://www.myget.org/F/cogenity/)

Symbols available from [SymbolSource](http://srv.symbolsource.org/pdb/MyGet/ibebbs/d58332dc-dffe-4da8-ba6a-8b1a73b759e9)
