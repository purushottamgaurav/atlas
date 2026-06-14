# Angular Interview Q&A

> Covers Angular fundamentals, architecture, RxJS, performance, security, and real-world patterns.

---

## Section 1: JavaScript & TypeScript Essentials

---

**Q1. What is the difference between `var`, `let`, and `const`?**

- **`var`** — function-scoped. Hoisted to the top of the function and initialized to `undefined`. Can be re-declared. Avoid in modern code.
- **`let`** — block-scoped. Hoisted but not initialized (accessing before declaration throws a `ReferenceError` — called the temporal dead zone). Cannot be re-declared in the same scope.
- **`const`** — block-scoped like `let`. Must be assigned at declaration. Cannot be reassigned. The object/array it points to can still be mutated.

Always prefer `const` by default, use `let` when you need to reassign, never use `var`.

---

**Q2. What is a closure in JavaScript?**

A closure is a function that remembers and can access variables from its outer scope even after the outer function has finished executing.

```javascript
function makeCounter() {
  let count = 0;
  return function () { return ++count; };
}
const counter = makeCounter();
counter(); // 1
counter(); // 2 — count is still remembered
```

Closures are the foundation of patterns like module encapsulation, memoization, and factory functions. In Angular, they appear in event handlers, RxJS operators, and service factories.

---

**Q3. What is event bubbling in JavaScript and how do you stop it?**

When a user clicks a button, the click event fires on that button first, then propagates (bubbles) up through every parent element to the root. A click handler on a parent `div` will also fire.

Stop it with `event.stopPropagation()`. Stop it and prevent the browser default (like form submission) with `event.preventDefault()`.

In Angular, use `$event.stopPropagation()` in template event bindings or the `@HostListener` decorator.

---

**Q4. What is the difference between `undefined` and `null`?**

- **`undefined`** — a variable was declared but never given a value. JavaScript assigns it automatically.
- **`null`** — an intentional "no value." A developer explicitly sets this.

`null == undefined` is `true` (loose equality). `null === undefined` is `false` (strict equality). `typeof null` is `'object'` — a well-known JavaScript quirk.

---

**Q5. What are `call`, `apply`, and `bind` in JavaScript?**

All three control what `this` refers to when calling a function.

- **`call`** — calls the function immediately. Pass arguments one by one: `fn.call(obj, arg1, arg2)`.
- **`apply`** — calls the function immediately. Pass arguments as an array: `fn.apply(obj, [arg1, arg2])`.
- **`bind`** — returns a new function with `this` permanently set. Does not call immediately: `const bound = fn.bind(obj); bound(arg1)`.

---

**Q6. What is the difference between `map`, `filter`, and `reduce`?**

All three are non-mutating array methods that return a new value.

- **`map`** — transforms every element. Returns a new array of the same length.
- **`filter`** — keeps only elements that pass a condition. Returns a shorter (or equal) array.
- **`reduce`** — accumulates all elements into a single value (sum, object, etc.).

```javascript
const nums = [1, 2, 3, 4, 5];
nums.filter(n => n > 2);               // [3, 4, 5]
nums.map(n => n * 2);                  // [2, 4, 6, 8, 10]
nums.reduce((sum, n) => sum + n, 0);   // 15
```

In Angular, these are used constantly inside RxJS pipe operators too.

---

**Q7. What is the non-null assertion operator (`!`) in TypeScript?**

The `!` after a value tells TypeScript "I know this is not null or undefined — trust me." It silences the TypeScript compiler warning without doing any runtime check.

```typescript
const el = document.getElementById('app')!; // TypeScript won't warn
el.style.display = 'none';
```

Use sparingly. If the value actually is null at runtime, you'll get a null reference error. Prefer a proper null check (`if (el)`) whenever possible.

---

**Q8. What is `async/await` and how does it relate to Promises?**

A `Promise` represents a future value. `async/await` is syntactic sugar over Promises — it lets you write asynchronous code that looks synchronous, making it much easier to read and debug.

```typescript
// Promise style
fetchUser().then(user => console.log(user)).catch(err => console.error(err));

// async/await style — same thing, cleaner
async function loadUser() {
  try {
    const user = await fetchUser();
    console.log(user);
  } catch (err) {
    console.error(err);
  }
}
```

In Angular, you'll sometimes mix `async/await` with Observables by converting with `firstValueFrom()` or `lastValueFrom()`.

---

## Section 2: Angular Core Concepts

---

**Q9. What is Angular and how is it different from AngularJS?**

Angular (v2+) is a complete rewrite. They share a name but are fundamentally different.

| Feature | AngularJS (v1) | Angular (v2+) |
|---|---|---|
| Language | JavaScript | TypeScript |
| Architecture | MVC, $scope | Component-based |
| Data flow | Two-way (digest cycle) | Unidirectional + two-way opt-in |
| Performance | Slower at scale | Ivy renderer, tree shaking |
| Mobile | Not designed for it | Mobile-first |
| CLI | No | Angular CLI |

Not backward compatible. If you see `ng-model` and `$scope` — that's AngularJS.

---

**Q10. What is the role of `NgModule` and what does `AppModule` do?**

`NgModule` is a container that groups related components, directives, pipes, and services. Every Angular app has at least one module — `AppModule`.

`AppModule` is the root module. It:
- Declares all root-level components.
- Imports `BrowserModule` (required for browser rendering).
- Bootstraps `AppComponent` (the first component Angular renders).
- Imports feature modules and third-party modules.

With standalone components (Angular 14+), you can skip NgModule entirely for many use cases.

---

**Q11. What is a component in Angular and what makes it up?**

A component is the basic building block of an Angular UI. Every visible part of the screen — header, sidebar, button group — is a component.

A component has:
- **Class** (`.ts`) — the logic and data.
- **Template** (`.html`) — the view, written in HTML + Angular syntax.
- **Styles** (`.css/.scss`) — scoped styles.
- **`@Component` decorator** — metadata that ties them together.

```typescript
@Component({
  selector: 'app-product-card',
  templateUrl: './product-card.component.html',
  styleUrls: ['./product-card.component.scss']
})
export class ProductCardComponent {
  @Input() product!: Product;
}
```

---

**Q12. What are the 4 types of data binding in Angular?**

- **Interpolation** `{{ value }}` — display a component value in the template. One-way (component → template).
- **Property binding** `[property]="value"` — set a DOM property from the component. One-way (component → DOM).
- **Event binding** `(event)="handler()"` — listen to DOM events and call a method. One-way (DOM → component).
- **Two-way binding** `[(ngModel)]="value"` — syncs the component and input field in both directions. Requires `FormsModule`. Under the hood it's property + event binding combined.

---

**Q13. How do you pass data between a parent and child component?**

- **Parent → Child** — use `@Input()` on the child's property. The parent passes the value in the template: `[childProp]="parentValue"`.
- **Child → Parent** — use `@Output()` with `EventEmitter` on the child. The child calls `emit()`. The parent listens: `(childEvent)="handler($event)"`.
- **Sibling or unrelated components** — use a shared service with `BehaviorSubject`.

```typescript
// Child component
@Input() title = '';
@Output() saved = new EventEmitter<string>();

onSave() { this.saved.emit('done'); }
```

```html
<!-- Parent template -->
<app-child [title]="myTitle" (saved)="onChildSaved($event)"></app-child>
```

---

**Q14. What are Angular lifecycle hooks? Name them in order.**

Lifecycle hooks let you run code at specific moments in a component's life.

1. **`ngOnChanges`** — fires when an `@Input` value changes. Before `ngOnInit`. Has a `SimpleChanges` argument.
2. **`ngOnInit`** — fires once after the first `ngOnChanges`. Best place for initialization logic (API calls, setup).
3. **`ngDoCheck`** — fires on every change detection run. Use for custom change detection logic.
4. **`ngAfterContentInit`** — fires once after `<ng-content>` (projected content) is initialized.
5. **`ngAfterContentChecked`** — fires after every check of projected content.
6. **`ngAfterViewInit`** — fires once after the component's view and all child views are initialized. Use to access `@ViewChild`.
7. **`ngAfterViewChecked`** — fires after every check of the component's view.
8. **`ngOnDestroy`** — fires just before the component is destroyed. Clean up subscriptions and timers here.

**Constructor vs ngOnInit:** Constructor is for dependency injection and class setup. `ngOnInit` is for Angular-specific initialization after bindings are set up.

---

**Q15. What are the types of directives in Angular?**

- **Component directive** — a directive with a template. Every `@Component` is technically a directive.
- **Structural directive** — changes the DOM structure by adding or removing elements. Built-in: `*ngIf`, `*ngFor`, `*ngSwitch`. The `*` is syntactic sugar for `<ng-template>`.
- **Attribute directive** — changes the appearance or behavior of an existing element without altering its structure. Built-in: `ngClass`, `ngStyle`. Custom ones use `@Directive`, `ElementRef`, and `Renderer2`.

---

**Q16. What is the difference between `ng-container`, `ng-content`, and `ng-template`?**

- **`ng-container`** — a grouping element that renders nothing in the DOM. Use it when you need to apply `*ngIf` or `*ngFor` without adding an extra `<div>` wrapper.
- **`ng-content`** — content projection slot. Whatever the parent puts inside your component's tags gets projected here. Like `slot` in Web Components.
- **`ng-template`** — a block of HTML that is not rendered by default. Used by `*ngIf` else clause, `*ngFor`, and `ViewContainerRef` for dynamic rendering.

---

**Q17. What is the difference between Reactive Forms and Template-Driven Forms?**

| Feature | Template-Driven | Reactive |
|---|---|---|
| Logic lives in | HTML template | Component class |
| Setup | `ngModel`, `FormsModule` | `FormGroup`, `ReactiveFormsModule` |
| Testing | Harder (needs DOM) | Easy (pure class logic) |
| Dynamic fields | Cumbersome | Easy with `FormArray` |
| Validation | HTML attributes | Validators in code |
| Use for | Simple forms | Complex, dynamic, testable forms |

For a .NET full stack dev: think of Template-Driven like minimal API endpoints — quick but limited. Reactive Forms are like controller-based — more structure, more control.

---

**Q18. What are built-in validators in Angular Reactive Forms?**

`Validators` class provides: `required`, `minLength`, `maxLength`, `min`, `max`, `email`, `pattern`, `nullValidator`.

```typescript
this.form = this.fb.group({
  email: ['', [Validators.required, Validators.email]],
  age: ['', [Validators.required, Validators.min(18), Validators.max(99)]],
  password: ['', [Validators.required, Validators.minLength(8)]]
});

// In template
<input formControlName="email" />
<div *ngIf="form.get('email')?.hasError('email')">Invalid email</div>
```

For cross-field validation (password match), write a custom validator function applied to the `FormGroup`.

---

**Q19. What is routing in Angular and how do you set it up?**

Angular Router lets users navigate between views without a full page reload (SPA behavior). You define a route map that links URL paths to components.

```typescript
const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'products', component: ProductListComponent },
  { path: 'products/:id', component: ProductDetailComponent },
  { path: '**', component: NotFoundComponent }  // wildcard
];
```

In the template, use `<router-outlet>` as the placeholder where routed components render, and `[routerLink]` for navigation links.

---

**Q20. What are route guards and what types exist?**

Route guards control whether a user can navigate to or away from a route.

- **`CanActivate`** — can the user access this route? (check login/role)
- **`CanActivateChild`** — same but for child routes.
- **`CanDeactivate`** — can the user leave? (warn about unsaved changes)
- **`CanLoad`** — should the lazy-loaded module even be downloaded?
- **`Resolve`** — prefetch data before the route activates so the component has data immediately.

```typescript
@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(private auth: AuthService, private router: Router) {}

  canActivate(): boolean {
    if (this.auth.isLoggedIn()) return true;
    this.router.navigate(['/login']);
    return false;
  }
}
```

---

**Q21. What are parameterized routes and how do you read route parameters?**

A parameterized route has a `:param` segment in the path. The value is available in the component via `ActivatedRoute`.

```typescript
// Route definition
{ path: 'products/:id', component: ProductDetailComponent }

// In component
constructor(private route: ActivatedRoute) {}

ngOnInit() {
  // Snapshot - use when the param won't change while on the same component
  const id = this.route.snapshot.params['id'];

  // Observable - use when navigating between instances of the same component
  this.route.params.subscribe(p => this.loadProduct(p['id']));
}
```

---

**Q22. What is lazy loading and how does it improve performance?**

By default, Angular loads all modules at startup (eager loading). Lazy loading delays loading a feature module until the user navigates to that route. This reduces the initial bundle size and speeds up app startup.

```typescript
const routes: Routes = [
  { path: 'admin', loadChildren: () =>
      import('./admin/admin.module').then(m => m.AdminModule) }
];
```

Angular also supports lazy-loaded standalone components (Angular 14+) with `loadComponent`.

---

## Section 3: Services, DI & RxJS

---

**Q23. What is Dependency Injection in Angular and how does it work?**

DI is a design pattern where a class receives its dependencies from the outside rather than creating them itself. Angular has a built-in hierarchical DI system.

When Angular sees a service type in a constructor, it looks it up in the injector hierarchy and provides an instance. You register services with `providedIn: 'root'` (singleton for the whole app) or in a specific module/component (scoped instance).

```typescript
@Injectable({ providedIn: 'root' })
export class ProductService {
  constructor(private http: HttpClient) {}
  getAll() { return this.http.get<Product[]>('/api/products'); }
}
```

---

**Q24. What is the difference between Promise and Observable?**

| Feature | Promise | Observable |
|---|---|---|
| Values | Single value | Stream of 0 to many values |
| Execution | Eager (runs immediately) | Lazy (runs on subscribe) |
| Cancellable | No | Yes (unsubscribe) |
| Operators | Limited (then/catch) | Full RxJS operator pipeline |
| Used in Angular | async/await, one-off calls | HttpClient, Router, Forms |

Use Observables in Angular — they fit the async, event-driven nature of the framework. Convert to Promise with `firstValueFrom()` when you need to `await` an Observable result.

---

**Q25. What is RxJS and what are its most commonly used operators?**

RxJS (Reactive Extensions for JavaScript) is a library for working with asynchronous data streams using Observables.

Key operators to know:

- **`map`** — transform each emitted value.
- **`filter`** — only emit values that pass a condition.
- **`tap`** — side effects (logging) without changing the stream.
- **`switchMap`** — cancel the previous inner Observable when a new value arrives. Used for search-as-you-type.
- **`mergeMap`** — run inner Observables in parallel without cancelling.
- **`concatMap`** — queue inner Observables and run them in order.
- **`catchError`** — handle errors and return a fallback Observable.
- **`debounceTime`** — wait for a pause before emitting. Good for search inputs.
- **`distinctUntilChanged`** — don't emit if the value is the same as the last one.
- **`forkJoin`** — wait for multiple Observables to complete and emit all results together.
- **`combineLatest`** — emit whenever any source emits, combining latest values.
- **`takeUntil`** — unsubscribe when another Observable emits.

---

**Q26. What is the difference between Subject, BehaviorSubject, ReplaySubject, and AsyncSubject?**

All four are both an Observable and an Observer — they emit values and you can subscribe to them.

- **`Subject`** — no initial value, no memory. New subscribers only get future emissions.
- **`BehaviorSubject`** — holds the **current value**. New subscribers immediately receive the last emitted value. Most useful for shared state.
- **`ReplaySubject(n)`** — replays the last `n` emissions to new subscribers.
- **`AsyncSubject`** — only emits the **last** value, and only when the source completes.

```typescript
// BehaviorSubject as a shared state store
private userSubject = new BehaviorSubject<User | null>(null);
user$ = this.userSubject.asObservable(); // expose as Observable (read-only)

setUser(user: User) { this.userSubject.next(user); }
```

---

**Q27. What are HTTP Interceptors and what are they used for?**

Interceptors sit in the middle of every HTTP request and response. Implement `HttpInterceptor` to intercept, modify, or react to all HTTP traffic in one place.

Common uses:
- **Auth** — attach the JWT Bearer token to every outgoing request.
- **Error handling** — catch 401 (redirect to login) or 500 (show toast) globally.
- **Loading spinner** — show spinner on request start, hide on response.
- **Logging** — log every request URL and response status.
- **Retry** — automatically retry failed requests.

```typescript
@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private auth: AuthService) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = this.auth.getToken();
    const authReq = token
      ? req.clone({ setHeaders: { Authorization: `Bearer ${token}` } })
      : req;
    return next.handle(authReq);
  }
}
```

Register in `AppModule` providers: `{ provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }`.

---

**Q28. How does a refresh token flow work in Angular?**

1. User logs in → server returns a short-lived access token and a long-lived refresh token.
2. Access token is stored in memory (or `localStorage`). Refresh token stored in an `HttpOnly` cookie.
3. When the access token expires, the API returns `401`.
4. An interceptor catches the `401`, calls the `/auth/refresh` endpoint with the refresh token.
5. Server returns a new access token. Interceptor retries the original request automatically.
6. The user never sees an error — the refresh is silent.
7. If the refresh token is also expired → redirect to login.

---

**Q29. What is the `async` pipe and why is it preferred over manual subscribe?**

The `async` pipe subscribes to an Observable (or Promise) in the template and automatically unsubscribes when the component is destroyed. This prevents memory leaks.

```typescript
// Component
products$ = this.productService.getAll(); // Observable<Product[]>
```

```html
<!-- Template - no subscribe/unsubscribe needed -->
<div *ngFor="let p of products$ | async">{{ p.name }}</div>
```

Without `async`, you'd need to manually subscribe in `ngOnInit` and unsubscribe in `ngOnDestroy` — forgetting to unsubscribe is a common memory leak.

---

**Q30. How do you prevent memory leaks from subscriptions in Angular?**

Three common patterns:

1. **`async` pipe** — auto-unsubscribes. Best for template subscriptions.
2. **`takeUntil`** — emit a destroy signal in `ngOnDestroy` to complete all subscriptions.
3. **`take(1)`** — automatically completes after receiving one emission. Good for one-off calls.

```typescript
// takeUntil pattern
private destroy$ = new Subject<void>();

ngOnInit() {
  this.dataService.getData()
    .pipe(takeUntil(this.destroy$))
    .subscribe(data => this.data = data);
}

ngOnDestroy() {
  this.destroy$.next();
  this.destroy$.complete();
}
```

---

## Section 4: Performance & Change Detection

---

**Q31. How does Angular change detection work?**

Angular's change detection checks if any component data has changed and updates the DOM accordingly. By default, Angular checks **every component in the entire tree** on every browser event, HTTP response, or timer.

**Zone.js** makes this happen automatically — it patches all async APIs (setTimeout, Promise, XHR) and tells Angular "something may have changed, please check."

For better performance, use `ChangeDetectionStrategy.OnPush` on components — Angular will only check them when an `@Input` reference changes or an event fires inside the component.

---

**Q32. What is `ChangeDetectionStrategy.OnPush` and when should you use it?**

With `OnPush`, Angular skips a component during change detection unless:
- One of its `@Input` properties received a **new object reference** (not a mutation).
- An event fired inside the component.
- An Observable bound with `async` pipe emits.
- You manually trigger it via `ChangeDetectorRef.markForCheck()`.

Use `OnPush` on any component that only depends on its inputs — especially list items, cards, and display-only components. It can dramatically reduce change detection work in large lists.

```typescript
@Component({
  selector: 'app-product-card',
  changeDetection: ChangeDetectionStrategy.OnPush,
  template: `<div>{{ product.name }}</div>`
})
export class ProductCardComponent {
  @Input() product!: Product;
}
```

---

**Q33. What is the difference between AOT and JIT compilation?**

- **JIT (Just-In-Time)** — templates are compiled in the browser at runtime. Slower startup, bigger bundle. Used in development.
- **AOT (Ahead-Of-Time)** — templates are compiled at build time by the Angular CLI. Faster rendering, smaller bundles, and template errors are caught at build time before deployment. Default for production (`ng build`).

When a client receives the app with AOT, the browser doesn't need to compile anything — it just runs pre-compiled code.

---

**Q34. What is tree shaking in Angular?**

Tree shaking is the process of removing unused code from the final bundle during the production build. The Angular CLI (via Webpack or esbuild) statically analyzes what's imported and used, and drops everything else.

This means if you import a module but only use one function, the unused code is not included in the bundle. Angular itself is built to be tree-shakable — features you don't use are stripped out.

Practical tip: import named exports instead of entire libraries (`import { debounceTime } from 'rxjs/operators'` not `import * as rxjs from 'rxjs'`).

---

**Q35. What is View Encapsulation in Angular?**

View Encapsulation controls how a component's CSS styles are scoped — whether they leak into other components.

- **`Emulated` (default)** — Angular adds unique attribute selectors to CSS rules to scope them to the component. Looks like Shadow DOM but isn't.
- **`None`** — no encapsulation. Styles become global and can affect any component.
- **`ShadowDom`** — uses the native browser Shadow DOM API for true encapsulation.

Use `None` carefully — it's like declaring global CSS in a component file.

---

**Q36. What are pure and impure pipes?**

- **Pure pipe** — Angular only runs it when the input **reference** changes. Default and efficient. Good for most transformations.
- **Impure pipe** — Angular runs it on **every change detection cycle**, regardless of whether the input changed. Slow if misused. The built-in `async` pipe is impure (needs to check for new values every cycle).

Avoid creating impure pipes for filtering or sorting large arrays — it runs constantly and kills performance. Instead, filter/sort in the component and store the result.

---

**Q37. What is lazy loading and how does it differ from preloading?**

- **Lazy loading** — the module JS is not downloaded until the user navigates to that route. Reduces initial bundle.
- **Preloading** — the module is lazy-loaded but Angular downloads it in the background after the app loads. The user gets fast initial load AND fast navigation.

```typescript
// Preloading strategy - loads lazy modules after app starts
RouterModule.forRoot(routes, { preloadingStrategy: PreloadAllModules })
```

You can also write a custom preloading strategy to only preload certain routes.

---

## Section 5: Architecture & State Management

---

**Q38. What are the types of Angular services and how are they scoped?**

A service is scoped based on where it's provided:

- **`providedIn: 'root'`** — singleton for the entire app. One shared instance.
- **Provided in a feature module** — shared across all components in that module, separate instance from root.
- **Provided in a component** — new instance created for that component and all its children. Destroyed with the component.

Use root for shared services (auth, logging, HTTP). Use component-level for services that hold state tied to a specific component instance.

---

**Q39. What is NgRx and why would you use it?**

NgRx is a state management library for Angular based on the Redux pattern. It provides a single immutable **store** for all application state.

Core concepts:
- **Store** — the single source of truth for state.
- **Action** — describes what happened (`[Cart] Add Item`).
- **Reducer** — a pure function that takes the current state + action and returns a new state.
- **Selector** — a function to read a slice of state.
- **Effect** — handles side effects (API calls) triggered by actions.

Use NgRx when: the app is large, many components share state, or debugging with Redux DevTools time-travel is valuable. For smaller apps, a service with `BehaviorSubject` is usually enough.

---

**Q40. How do you share state between unrelated components without NgRx?**

Use a shared service with a `BehaviorSubject`:

```typescript
@Injectable({ providedIn: 'root' })
export class CartService {
  private cartItems = new BehaviorSubject<CartItem[]>([]);
  cartItems$ = this.cartItems.asObservable();

  addItem(item: CartItem) {
    this.cartItems.next([...this.cartItems.getValue(), item]);
  }
}
```

Any component that injects `CartService` and subscribes to `cartItems$` will get live updates. This covers 80% of state management needs without NgRx complexity.

---

**Q41. What is Angular Universal (SSR) and why would you use it?**

Angular Universal enables **Server-Side Rendering** — the HTML is generated on the server and sent to the browser, instead of the browser downloading JS and rendering it.

Benefits:
- **Better SEO** — search engine crawlers see full HTML, not an empty `<app-root>`.
- **Faster first paint** — users see content immediately before JS loads.
- **Social media sharing** — Open Graph tags are rendered server-side.

Downside: more complex infrastructure (Node.js server required). For apps that don't need SEO (internal dashboards), CSR is simpler.

---

**Q42. What are standalone components in Angular 14+ and why are they useful?**

Standalone components don't belong to any `NgModule`. They declare their own imports directly. This reduces boilerplate and makes components more self-contained.

```typescript
@Component({
  standalone: true,
  selector: 'app-product-card',
  imports: [CommonModule, RouterModule],  // directly import what you need
  template: `<a [routerLink]="['/products', product.id]">{{ product.name }}</a>`
})
export class ProductCardComponent {
  @Input() product!: Product;
}
```

Angular 17+ uses standalone components as the default. New projects no longer need `AppModule`.

---

**Q43. How do you handle errors from HTTP calls globally in Angular?**

Use an interceptor to catch errors once for the whole app, rather than writing `catchError` in every service method.

```typescript
@Injectable()
export class ErrorInterceptor implements HttpInterceptor {
  constructor(private notification: NotificationService, private router: Router) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) this.router.navigate(['/login']);
        else if (error.status === 403) this.router.navigate(['/forbidden']);
        else this.notification.showError('Something went wrong');
        return throwError(() => error);
      })
    );
  }
}
```

---

**Q44. How do you implement route-based code splitting for better performance?**

Every lazy-loaded route automatically creates a separate JS chunk. The browser only downloads the chunk when the user navigates to that route.

For even better performance:
- Split large feature modules into smaller sub-modules.
- Use `loadComponent` for standalone components (Angular 14+).
- Use a custom preloading strategy to preload important routes in the background after startup.
- Analyze bundle sizes with `ng build --stats-json` and Webpack Bundle Analyzer.

---

## Section 6: Security & Real-World Patterns

---

**Q45. How do you protect routes that require authentication in Angular?**

Use `CanActivate` guard. Check the auth state in the guard and redirect to login if not authenticated.

For role-based access, read the user's roles from the JWT claims stored in the auth service:

```typescript
canActivate(route: ActivatedRouteSnapshot): boolean {
  const required = route.data['role'];
  if (!this.auth.isLoggedIn()) {
    this.router.navigate(['/login']);
    return false;
  }
  if (required && !this.auth.hasRole(required)) {
    this.router.navigate(['/forbidden']);
    return false;
  }
  return true;
}
```

---

**Q46. How do you prevent XSS attacks in Angular?**

Angular protects against XSS by default — it sanitizes all values bound to the DOM through interpolation `{{ }}` and property bindings. Dangerous HTML is escaped automatically.

If you genuinely need to render HTML, use `DomSanitizer.bypassSecurityTrustHtml()` — but only for content you control. Never pass user input through it.

Avoid using `innerHTML` binding with user data. Never disable Angular's sanitization without a specific reason.

---

**Q47. What is CSRF and how does Angular handle it?**

CSRF (Cross-Site Request Forgery) tricks a logged-in user's browser into making unwanted requests. Angular's `HttpClient` automatically reads a cookie named `XSRF-TOKEN` and sends it as a header (`X-XSRF-TOKEN`) on every non-GET request. The server validates this header.

Your .NET backend sets this cookie and validates the header — ASP.NET Core has built-in anti-forgery support. If your API uses stateless JWT (no cookies for auth), CSRF is generally not a concern.

---

**Q48. How do you implement search-as-you-type with RxJS?**

Use `debounceTime` + `distinctUntilChanged` + `switchMap`. This waits for the user to stop typing, skips unchanged values, and cancels previous API calls if the user types again quickly.

```typescript
searchControl = new FormControl('');

results$ = this.searchControl.valueChanges.pipe(
  debounceTime(300),           // wait 300ms after last keystroke
  distinctUntilChanged(),      // skip if same value as last
  filter(term => term.length > 2), // don't search for 1-2 chars
  switchMap(term =>            // cancel previous, start new search
    this.productService.search(term).pipe(catchError(() => of([])))
  )
);
```

```html
<input [formControl]="searchControl" placeholder="Search..." />
<div *ngFor="let r of results$ | async">{{ r.name }}</div>
```

---

**Q49. What is the difference between `@ViewChild` and `@ContentChild`?**

- **`@ViewChild`** — gives access to a child element or component defined in the **component's own template**.
- **`@ContentChild`** — gives access to content **projected into** the component via `<ng-content>` from the parent.

```typescript
// ViewChild - element in this component's own template
@ViewChild('myInput') inputRef!: ElementRef;
ngAfterViewInit() { this.inputRef.nativeElement.focus(); }

// ContentChild - content projected by the parent
@ContentChild(TabTitleComponent) tabTitle!: TabTitleComponent;
ngAfterContentInit() { console.log(this.tabTitle.title); }
```

---

**Q50. What is the Angular upgrade path and how do you keep a large app up to date?**

Angular releases a major version every 6 months with 18 months of LTS support. The recommended upgrade approach:

1. Check **update.angular.io** for a guide between your current and target version.
2. **Upgrade one major version at a time** (v15 → v16, not v15 → v18).
3. Run `ng update @angular/core @angular/cli` — this updates packages and applies schematics that automatically fix breaking changes.
4. Fix any remaining TypeScript errors and test thoroughly.
5. Update third-party Angular libraries (`Angular Material`, `NgRx`, etc.) after the core update.

For large apps: maintain a test suite that runs after each upgrade step. Use Angular's deprecation warnings early — they appear 1-2 major versions before removal.

---