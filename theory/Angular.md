# Angular & JavaScript Interview Q&A

---

## JavaScript

1. **Difference between var and let?**
var is function-scoped, hoisted to the top of the function, and can be re-declared. let is block-scoped, not accessible before declaration (temporal dead zone), and cannot be re-declared in the same scope. const is also block-scoped and cannot be reassigned. Prefer let and const over var.

2. **What are closures in JavaScript?**
A closure is a function that remembers the variables from its outer scope even after the outer function has returned. The inner function retains access to the outer function's variables.
```javascript
function counter() {
  let count = 0;
  return function() { return ++count; };
}
const inc = counter();
inc(); // 1
inc(); // 2
```

3. **3 ways to invoke a function — call, apply, bind?**
call — invokes the function immediately with a specified this and individual arguments. fn.call(obj, arg1, arg2).
apply — same as call but arguments are passed as an array. fn.apply(obj, [arg1, arg2]).
bind — returns a new function with this permanently bound. Does not invoke immediately. const bound = fn.bind(obj); bound(arg1);

4. **What is variable hoisting in JavaScript?**
Hoisting moves variable and function declarations to the top of their scope at compile time. var declarations are hoisted and initialized to undefined. let and const are hoisted but not initialized — accessing them before declaration throws a ReferenceError (temporal dead zone). Function declarations are fully hoisted (both declaration and body).

5. **What are scopes, prototypes, closures?**
Scope — determines where a variable is accessible. Types: global, function, block. Lexical scoping means inner functions access outer variables.
Prototype — every JavaScript object has a prototype chain. Properties/methods not found on an object are looked up on its prototype. Basis of inheritance in JS.
Closure — a function that captures variables from its enclosing scope. (See Q2 above.)

6. **Difference between $.ajax vs $.post?**
$.ajax is the full-featured jQuery AJAX method — allows configuring method, headers, data type, success/error callbacks, and more.
$.post is a shorthand for $.ajax with method set to POST. Less configuration, less flexible. $.get is the GET equivalent.

7. **Minimum parameters for an AJAX call?**
The only required parameter is url. Method defaults to GET, dataType is inferred, and callbacks are optional.
```javascript
$.ajax({ url: '/api/data' });
```

8. **Difference between $(document).ready() and $(function)?**
They are identical. $(function() { }) is just a shorthand for $(document).ready(function() { }). Both execute the callback after the DOM is fully loaded but before images are loaded.

9. **What is Event Bubbling?**
When an event fires on an element, it propagates (bubbles) up through parent elements in the DOM. A click on a button will also trigger click handlers on its parent div, body, and document. Use event.stopPropagation() to prevent bubbling. The opposite is event capturing (top-down). addEventListener has a third parameter to control capture vs bubble phase.

10. **Ajax in jQuery — get textbox value using JavaScript?**
```javascript
// Get value
var val = document.getElementById('myInput').value;
// OR jQuery
var val = $('#myInput').val();

// AJAX with value
$.ajax({
  url: '/api/save', method: 'POST',
  data: { name: val },
  success: function(res) { console.log(res); }
});
```

11. **undefined vs null?**
undefined — variable declared but not assigned a value. typeof undefined is 'undefined'.
null — intentional absence of a value. Explicitly assigned. typeof null is 'object' (a known JS quirk).
null == undefined is true (loose equality). null === undefined is false (strict equality).

12. **What is non-null assertion in Angular/TypeScript (!)?**
The non-null assertion operator (!) tells TypeScript to treat a value as non-null/non-undefined even if the type includes null or undefined. Use it when you are certain the value exists but TypeScript cannot infer it.
```typescript
const el = document.getElementById('app')!; // tells TS: not null
```
Overuse is a code smell — prefer proper null checks where possible.

13. **let vs var (summary)?**
var — function scope, hoisted, can redeclare, no block scope. let — block scope, not hoisted (TDZ), cannot redeclare. In Angular/TypeScript, always use let or const. var should be avoided.

---

## Angular

14. **Promise vs Observable vs Subject vs BehaviorSubject?**
Promise — single future value, eager (executes immediately), not cancellable.
Observable — stream of values, lazy (executes on subscribe), cancellable, supports operators (map, filter, etc.). From RxJS.
Subject — both an Observable and an Observer. Multicasts to multiple subscribers. Does not hold a value.
BehaviorSubject — like Subject but holds the current value. New subscribers immediately receive the last emitted value. Used for shared state.

15. **Routes, parameterized routes, named router-outlet, routerLink, routerState, Auth Guards?**
Basic route — { path: 'home', component: HomeComponent }.
Parameterized route — { path: 'user/:id', component: UserComponent }. Read with ActivatedRoute.snapshot.params['id'].
Named router-outlet — <router-outlet name="sidebar"> for multiple outlets in a layout.
routerLink — directive for navigation: [routerLink]="['/user', id]".
RouterState — represents the current state of the router and the URL tree.
Auth Guards — CanActivate (prevent access to route), CanActivateChild (child routes), CanDeactivate (warn on unsaved changes), CanLoad (prevent lazy module from loading), Resolve (prefetch data before route activates).

16. **Reactive Forms vs Template-Driven Forms?**
Template-Driven — simpler, logic in HTML using ngModel. Good for simple forms. Less control.
Reactive Forms — logic in the component class using FormGroup, FormControl, FormArray. More explicit, testable, and scalable. Preferred for complex forms with dynamic fields or custom validation.

17. **What are interceptors in Angular?**
Interceptors sit between the app and the HTTP layer. They intercept outgoing requests and incoming responses. Implement HttpInterceptor interface with intercept() method.
Types — Authentication (attach JWT token to headers), Logging (log request/response), Error Handling (global HTTP error handling), Loading Spinner (show/hide on request start/end).
Yes, a single interceptor can log both request and response by tapping before and after calling next.handle(req).

18. **Can a single interceptor log both request and response?**
Yes. Use tap operator on the observable returned by next.handle(req).
```typescript
intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
  console.log('Request:', req.url);
  return next.handle(req).pipe(
    tap(event => { if (event instanceof HttpResponse) console.log('Response:', event.status); })
  );
}
```

19. **View Encapsulation and its types?**
Controls how component styles are scoped to prevent leaking to other components.
Emulated (default) — Angular adds unique attributes to elements and rewrites CSS selectors to scope them. No native Shadow DOM.
None — no style encapsulation. Styles are global.
ShadowDom — uses native browser Shadow DOM for true encapsulation.

20. **What are templates in Angular?**
Templates define the HTML view of a component. Static/inline — defined in the template property of @Component. Dynamic/separated — defined in a separate .html file referenced by templateUrl. Templates can contain bindings, directives, pipes, and Angular-specific syntax.

21. **Components, Modules, and Services in Angular?**
Component — building block of the UI. Has template, styles, and logic. Decorated with @Component.
Module — groups related components, directives, pipes, and services. @NgModule. AppModule bootstraps the app. Feature modules organize functionality.
Service — shared logic/data layer. Decorated with @Injectable. Common built-in services: HttpClient, Router, ActivatedRoute, Title, Location.

22. **Scopes in Angular?**
Angular does not have AngularJS-style $scope. Instead, data binding scope is the component class itself. Each component has its own scope. Services with providedIn: 'root' are singleton (app-wide scope). Services provided in a module or component have that module/component's scope.

23. **4 types of data binding in Angular?**
Interpolation — {{ value }} — component to template, one-way.
Property Binding — [property]="value" — component to DOM, one-way.
Event Binding — (event)="handler()" — DOM to component, one-way.
Two-way Binding — [(ngModel)]="value" — bidirectional sync between component and template. Requires FormsModule.

24. **Share data from Parent to Child and Child to Parent?**
Parent to Child — use @Input() decorator on child property. Parent passes data via [childProp]="parentValue".
Child to Parent — use @Output() with EventEmitter. Child emits an event; parent listens with (childEvent)="handler($event)".
Sibling/Unrelated — use a shared service with BehaviorSubject.

25. **Types of decorators in Angular?**
Class decorators — @Component, @NgModule, @Injectable, @Directive, @Pipe.
Property decorators — @Input, @Output, @ViewChild, @ViewChildren, @ContentChild, @HostBinding.
Method decorators — @HostListener (listen to DOM events on the host element).
Parameter decorators — @Inject, @Host, @Self, @SkipSelf, @Optional (control DI resolution).

26. **Angular lifecycle hooks — all 8?**
ngOnChanges — called when @Input values change. Before ngOnInit.
ngOnInit — called once after first ngOnChanges. Use for initialization logic.
ngDoCheck — called on every change detection cycle. Use for custom change detection.
ngAfterContentInit — called once after content projection (ng-content) is initialized.
ngAfterContentChecked — called after every check of projected content.
ngAfterViewInit — called once after component's view and child views are initialized. Use to access ViewChild.
ngAfterViewChecked — called after every check of the view.
ngOnDestroy — called just before component is destroyed. Clean up subscriptions, timers.
Difference from constructor — constructor is for DI and class initialization. Lifecycle hooks are for Angular-specific logic after Angular sets up the component.

27. **Pure vs Impure Pipe, custom pipes, built-in pipes, pipe chaining?**
Pure pipe — only runs when the input reference changes. Default. Efficient.
Impure pipe — runs on every change detection cycle. Use sparingly (e.g., async pipe is impure).
Custom pipe — implement PipeTransform interface with transform() method. Decorate with @Pipe.
Built-in pipes — DatePipe, CurrencyPipe, DecimalPipe, PercentPipe, UpperCasePipe, LowerCasePipe, JsonPipe, AsyncPipe, SlicePipe.
Pipe chaining — {{ value | date | uppercase }} — output of one pipe is input to the next.

28. **Directives — types?**
Component directive — a directive with a template. The most common.
Structural directive — changes the DOM structure. *ngIf, *ngFor, *ngSwitch. Prefix * is syntactic sugar for ng-template.
Attribute directive — changes appearance or behavior of an element. ngClass, ngStyle. Custom attribute directives use @Directive with ElementRef and Renderer2.

29. **Eager vs Lazy Loading, Transpiling?**
Eager Loading — all modules are loaded at startup. Increases initial load time. Default behavior.
Lazy Loading — modules are loaded on demand when the route is first accessed. Improves startup performance. Use loadChildren in route config.
Transpiling — TypeScript (.ts) is compiled to JavaScript (.js) by the TypeScript compiler (tsc) as part of the Angular build process.

30. **ng-container vs ng-content vs ng-template?**
ng-container — a logical grouping element that doesn't render any DOM element. Use with *ngIf or *ngFor when you don't want an extra wrapper element.
ng-content — content projection. Projects content from parent into a child component's template. Like a slot in web components.
ng-template — defines a template block that is not rendered by default. Used with *ngIf, *ngFor, or ViewContainerRef for dynamic rendering.

31. **filter, map, reduce in JavaScript?**
filter — returns a new array with elements that pass the condition. arr.filter(x => x > 2).
map — returns a new array by transforming each element. arr.map(x => x * 2).
reduce — reduces array to a single value by accumulating. arr.reduce((acc, x) => acc + x, 0).
All three are non-mutating. Commonly used with Observable streams in RxJS too.

32. **What is RxJS in Angular?**
RxJS (Reactive Extensions for JavaScript) is a library for reactive programming using Observables. Angular uses it extensively — HttpClient returns Observables, Router events are Observables. Key operators: map, filter, tap, switchMap, mergeMap, catchError, debounceTime, distinctUntilChanged, combineLatest, forkJoin, take, takeUntil.

33. **State management — NgRx and NGXS?**
NgRx — Redux pattern for Angular. Uses Actions, Reducers, Store, Effects, and Selectors. Verbose but powerful. Best for large, complex apps.
NGXS — simpler state management. Uses classes with decorators instead of plain objects. Less boilerplate than NgRx. Good for mid-size apps.
Both provide a single source of truth, predictable state, and DevTools support.

34. **Dependency Injection in Angular?**
Angular has its own hierarchical DI system. Services are registered in providers array (component, module, or root). Angular's injector creates and caches instances and injects them via constructor. providedIn: 'root' makes a service a singleton for the whole app. Child injectors can override parent services.

35. **AOT vs JIT compilation?**
JIT (Just-In-Time) — compiles TypeScript and templates in the browser at runtime. Slower startup, larger bundle. Used in development.
AOT (Ahead-Of-Time) — compiles templates and TypeScript at build time. Faster rendering, smaller bundles, earlier template error detection. Default in production builds (ng build --prod).

36. **Change detection — how does it work?**
Angular's change detection checks if the component's data has changed and updates the DOM. Default strategy — checks every component in the tree on every event/async operation. OnPush strategy — only checks when @Input references change or an event occurs inside the component. More performant. Zone.js patches async operations (setTimeout, XHR, Promises) to trigger change detection automatically. Use ChangeDetectorRef to manually trigger or detach.

37. **What is bootstrapping module?**
The root module that Angular uses to launch the application. Typically AppModule. Defined in main.ts via platformBrowserDynamic().bootstrapModule(AppModule). It must declare the root component (AppComponent) and import BrowserModule.

38. **How to handle errors in Observables and Subjects?**
Use catchError operator in the pipe to handle errors gracefully and return a fallback observable.
Use throwError to propagate custom errors. In subscribe, pass an error callback as the second argument.
```typescript
this.http.get('/api').pipe(
  catchError(err => { console.error(err); return of([]); })
).subscribe(data => {}, err => console.error(err));
```

39. **What type of DOM does Angular use? Benefits?**
Angular uses the Virtual DOM concept via its own change detection and rendering layer — though technically it works with the real DOM via a renderer abstraction. Angular Ivy renders directly and efficiently to the real DOM. Benefits: fast updates by minimizing actual DOM manipulation, platform abstraction (can render to server, web worker, native).

40. **Client-side rendering vs Angular Universal?**
Angular default — CSR (Client-Side Rendering). The browser downloads the JS bundle and renders the app. Fast interaction, slower first load.
Angular Universal — SSR (Server-Side Rendering). HTML is pre-rendered on the server and sent to the browser. Faster initial load, better SEO. The app then hydrates on the client.

41. **package.json vs package-lock.json?**
package.json — defines dependencies with version ranges (e.g., ^17.0.0 means >= 17.0.0 < 18.0.0). Maintained by the developer.
package-lock.json — locks the exact versions of every installed package and its sub-dependencies. Ensures consistent installs across machines. Committed to source control. Never edit manually.

42. **Types of JSON files in Angular?**
package.json — dependencies and scripts. package-lock.json — exact dependency versions. angular.json — Angular CLI configuration (build, serve, test options, assets, styles). tsconfig.json — TypeScript compiler options. tsconfig.app.json — app-specific TS config. tsconfig.spec.json — test-specific TS config. .browserslistrc — target browsers for compilation.

43. **How to call a component method from another component?**
Shared service — the cleanest approach. Inject the same service into both components; use a Subject or BehaviorSubject to communicate.
ViewChild — if one component is a child, the parent can call its methods via @ViewChild.
Event binding — child emits via @Output, parent handles the event.

44. **How to restrict a user from accessing a component?**
Use Route Guards — implement CanActivate. Check user role/permission in canActivate() and return true or false (or redirect with Router). For finer control inside a component, use interceptors to handle 401/403 HTTP responses and redirect to login.

45. **Observable collection and async pipe?**
Declare an Observable property in the component and assign an HTTP call or Subject to it. In the template use the async pipe — it subscribes automatically and handles unsubscription on component destroy.
```typescript
users$ = this.http.get<User[]>('/api/users');
// Template: *ngFor="let u of users$ | async"
```

46. **Data types supported by two-way binding?**
Two-way binding with ngModel works with string, number, and boolean. For complex objects, bind individual properties or use reactive forms. Two-way binding with custom components works with any type using @Input and @Output with the same name + Change convention.

47. **How to exclude unused code while compiling? Tree shaking?**
Tree shaking is done automatically during production build (ng build). The Angular CLI uses Webpack/esbuild which statically analyzes imports and removes unused code (dead code elimination). Ensure you import only what you need — avoid importing entire libraries when only one function is needed.

48. **Sorting and filtering for a table?**
Client-side — use Angular pipes (custom filter/sort pipe) or array methods in the component. For large datasets avoid impure pipes.
Server-side — pass sort and filter params to the API and let the backend handle it. Preferred for large datasets.
Libraries — Angular Material Table with MatSort and MatPaginator, or AG Grid, PrimeNG DataTable.

49. **How does refresh token work? Will the user see errors after token expiration?**
On login, the server returns both an access token (short-lived) and a refresh token (long-lived). When the access token expires, an interceptor catches the 401 response, uses the refresh token to silently get a new access token, and retries the original request. The user does not see any error — the process is transparent. If the refresh token also expires, the user is redirected to the login page.

50. **How does Angular achieve SPA? How to load only a particular section?**
Angular is a SPA — it loads a single index.html and dynamically swaps content using the Router without full page reloads. router-outlet is the placeholder where routed components are rendered. Only the matched component view is updated, not the entire page. Lazy loading ensures only the needed module's JS is loaded for a given route.

51. **How to upgrade Angular version?**
Use the official Angular Update Guide at update.angular.io. Steps: run ng update @angular/core @angular/cli to update to the next major version. Update one major version at a time. Fix breaking changes and deprecations after each step. Run ng update to check for other outdated packages. Test thoroughly after each upgrade.

52. **AngularJS vs Angular?**
AngularJS (1.x) — JavaScript-based, uses $scope, two-way data binding via digest cycle, not component-based, harder to scale, no CLI.
Angular (2+) — TypeScript-based, component-based architecture, hierarchical DI, unidirectional data flow, Angular CLI, Ivy renderer, better performance, mobile support. Completely rewritten — not backward compatible with AngularJS.

53. **Types of templates in Angular?**
Inline template — defined in the template property of @Component as a string. Good for small components.
External template — defined in a separate .html file referenced by templateUrl. Preferred for any non-trivial UI. Both support the full Angular template syntax — bindings, directives, pipes.