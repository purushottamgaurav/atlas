# Angular Interview Q&A 

> Beginner → Intermediate, in sequence. All core Angular concepts covered: JS/TS foundations → Angular intro → components → templates → directives → pipes → component communication → forms → routing → services & DI → HTTP → RxJS → performance → state management → security & build. Use this as a final lookup.

---

## Part 1: JavaScript Foundations

---

**Q1. What is the difference between `var`, `let`, and `const`?**

- **`var`** — function-scoped, hoisted, can be re-declared. Avoid in modern code.
- **`let`** — block-scoped, can be reassigned, cannot be re-declared in the same scope.
- **`const`** — block-scoped, cannot be reassigned. The object/array it points to can still be mutated.

Rule: use `const` by default, `let` when you need to reassign, never `var`.

---

**Q2. What is a closure?**

A function that "remembers" variables from its outer scope even after the outer function returned.

```javascript
function makeCounter() {
  let count = 0;
  return () => ++count;
}
const c = makeCounter();
c(); // 1
c(); // 2 — count is still alive
```

Used in event handlers, factories, and almost every RxJS callback.

---

**Q3. What is the difference between `undefined` and `null`?**

- **`undefined`** — variable declared but never assigned. JavaScript sets this automatically.
- **`null`** — explicit "no value" set by you.

`null == undefined` is `true` (loose). `null === undefined` is `false` (strict).

---

**Q4. What are `call`, `apply`, and `bind`?**

All control what `this` refers to.

- **`call`** — calls immediately, args one by one: `fn.call(obj, a, b)`.
- **`apply`** — calls immediately, args as array: `fn.apply(obj, [a, b])`.
- **`bind`** — returns a new function with `this` permanently set. Doesn't call.

---

**Q5. What is the difference between `map`, `filter`, and `reduce`?**

All return a new value (non-mutating).

- **`map`** — transforms each element. Same length.
- **`filter`** — keeps elements that match a condition. Shorter or same.
- **`reduce`** — collapses into a single value.

```javascript
[1,2,3,4,5].filter(n => n > 2);         // [3,4,5]
[1,2,3,4,5].map(n => n * 2);            // [2,4,6,8,10]
[1,2,3,4,5].reduce((s, n) => s + n, 0); // 15
```

---

**Q6. What is destructuring, and what are spread and rest operators?**

- **Destructuring** — pull values out of arrays/objects.
- **Spread (`...`)** — expand an array/object.
- **Rest (`...`)** — collect remaining args/items.

```typescript
const { name, age } = user;                 // destructure
const newArr = [...arr1, ...arr2];          // spread
function sum(...nums) { /* nums is array */ } // rest
```

---

**Q7. What is the difference between Promises and `async/await`?**

A Promise is a future value. `async/await` is cleaner syntax over the same thing — code reads top-to-bottom.

```typescript
// Promise
fetchUser().then(u => console.log(u)).catch(err => ...);

// async/await — same thing, easier to read
async function load() {
  try { const u = await fetchUser(); console.log(u); }
  catch (err) { /* ... */ }
}
```

---

**Q8. What is the difference between arrow functions and regular functions?**

- **Regular function** — has its own `this`. Can be used as a constructor (`new`).
- **Arrow function** — inherits `this` from the surrounding scope. Cannot be a constructor. Shorter syntax.

Use arrow functions for callbacks (event handlers, RxJS) so `this` keeps pointing to the component.

---

## Part 2: TypeScript Foundations

---

**Q9. What is TypeScript and why use it in Angular?**

TypeScript is JavaScript + static type checking. Angular is written in TypeScript.

Benefits:
- Catches errors at compile time, not runtime.
- IDE autocomplete and refactoring.
- Self-documenting code (types describe intent).
- Cleaner OOP features (interfaces, generics, access modifiers).

---

**Q10. What are type annotations?**

You tell TypeScript the expected type of variables, parameters, and return values.

```typescript
let count: number = 5;
function greet(name: string): string { return `Hi, ${name}`; }
const users: User[] = [];
```

If types don't match, TypeScript errors at build time.

---

**Q11. What is the difference between `interface` and `type`?**

Both describe the shape of an object. Mostly interchangeable.

- **`interface`** — can be extended (`extends`) and merged. Preferred for object shapes.
- **`type`** — can also describe primitives, unions, intersections, tuples. More flexible.

```typescript
interface User { id: number; name: string; }
type Status = 'active' | 'inactive';
```

---

**Q12. What are generics in TypeScript?**

Generics let you write reusable code that works with multiple types while keeping type safety.

```typescript
function identity<T>(value: T): T { return value; }

identity<string>('hi');   // returns string
identity<number>(5);      // returns number
```

In Angular: `Observable<User>`, `HttpClient.get<User[]>()`, `EventEmitter<string>`.

---

**Q13. What is an `enum`?**

A named set of constants.

```typescript
enum Role { Admin, User, Guest }
let role: Role = Role.Admin;     // 0
```

You can also define string enums: `enum Status { Active = 'ACTIVE' }`.

---

**Q14. What are decorators?**

Functions that add metadata to classes, methods, or properties using `@` syntax. Angular uses them everywhere: `@Component`, `@Injectable`, `@Input`, `@Output`, `@NgModule`.

```typescript
@Component({ selector: 'app-foo', template: '...' })
export class FooComponent { }
```

---

**Q15. What are the non-null assertion (`!`) and optional chaining (`?.`) operators?**

- **`!`** — tell TypeScript "trust me, this isn't null." No runtime check.
- **`?.`** — safely access a property. Returns `undefined` if anything in the chain is null/undefined.

```typescript
const len = user?.address?.street?.length;  // safe, returns undefined if any is null
const el = document.getElementById('app')!; // I know it's there
```

---

## Part 3: Angular Introduction

---

**Q16. What is Angular?**

A TypeScript-based framework by Google for building single-page applications (SPAs). It provides components, routing, forms, HTTP, DI, and tooling out of the box.

---

**Q17. What is the difference between Angular and AngularJS?**

| Feature | AngularJS (v1) | Angular (v2+) |
|---|---|---|
| Language | JavaScript | TypeScript |
| Architecture | MVC, `$scope` | Component-based |
| Data flow | Two-way (digest cycle) | Unidirectional + opt-in two-way |
| Performance | Slower | Ivy renderer, tree shaking |
| Tooling | None | Angular CLI |

Not backward compatible — Angular 2+ was a complete rewrite.

---

**Q18. What is Angular CLI and what are common commands?**

Command-line tool to scaffold, build, run, and deploy Angular apps.

```bash
ng new my-app                  # create new project
ng serve                       # run dev server (http://localhost:4200)
ng generate component product  # or "ng g c product"
ng generate service api        # generate a service
ng build                       # build for development
ng build --configuration production
ng test                        # run unit tests
ng lint                        # lint
ng update                      # update Angular versions
```

---

**Q19. What is the typical Angular project structure?**

```
my-app/
├── src/
│   ├── app/
│   │   ├── app.component.ts/html/css
│   │   ├── app.module.ts
│   │   └── feature/  ← your modules/components
│   ├── assets/        ← images, static files
│   ├── environments/  ← dev/prod config
│   ├── index.html
│   ├── main.ts        ← bootstrap entry point
│   └── styles.css     ← global styles
├── angular.json       ← CLI config
├── package.json
└── tsconfig.json
```

---

**Q20. What does `main.ts` do?**

It's the entry point of an Angular app. It bootstraps the root module (or root standalone component).

```typescript
// main.ts (NgModule-based)
platformBrowserDynamic().bootstrapModule(AppModule);

// main.ts (Standalone component, Angular 14+)
bootstrapApplication(AppComponent, { providers: [...] });
```

`index.html` contains `<app-root></app-root>`; Angular replaces it with `AppComponent`.

---

**Q21. What is `NgModule` and what does `AppModule` do?**

An `NgModule` is a container that groups related components, directives, pipes, and services.

`AppModule` is the root module:
- Declares root components.
- Imports `BrowserModule` (required for browser rendering).
- Bootstraps `AppComponent`.
- Imports feature modules and third-party modules.

```typescript
@NgModule({
  declarations: [AppComponent],
  imports: [BrowserModule, FormsModule, HttpClientModule],
  bootstrap: [AppComponent]
})
export class AppModule { }
```

---

**Q22. What are standalone components?**

Components that don't need `NgModule` — they declare their own imports. Less boilerplate. Default in Angular 17+.

```typescript
@Component({
  standalone: true,
  selector: 'app-product-card',
  imports: [CommonModule, RouterModule],
  template: `<a [routerLink]="['/products', product.id]">{{ product.name }}</a>`
})
export class ProductCardComponent {
  @Input() product!: Product;
}
```

---

## Part 4: Components

---

**Q23. What is a component in Angular?**

A component is a self-contained piece of UI — a class + template + styles. Every visible part of an Angular app is a component (header, button group, list, page).

---

**Q24. What does the `@Component` decorator contain?**

Metadata that tells Angular how to use the class.

```typescript
@Component({
  selector: 'app-product',           // tag name in HTML
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  encapsulation: ViewEncapsulation.Emulated
})
export class ProductComponent { }
```

You can use `template` and `styles` (inline) instead of `templateUrl` / `styleUrls` (file).

---

**Q25. What are the different types of component selectors?**

- **Element**: `selector: 'app-product'` → `<app-product>` (most common).
- **Attribute**: `selector: '[appProduct]'` → `<div appProduct>`.
- **Class**: `selector: '.app-product'` → `<div class="app-product">`.

---

**Q26. What is the difference between inline and external templates?**

- **Inline (`template`)** — HTML written in the TS file. Good for tiny components.
- **External (`templateUrl`)** — separate `.html` file. Better for anything non-trivial — editor support and clean separation.

Same applies to `styles` vs `styleUrls`.

---

**Q27. What is View Encapsulation in Angular?**

Controls how a component's CSS is scoped.

- **`Emulated` (default)** — Angular adds unique attributes to scope CSS to this component.
- **`None`** — styles become global; affect any component.
- **`ShadowDom`** — uses native browser Shadow DOM for real encapsulation.

Use `None` cautiously — it leaks styles app-wide.

---

**Q28. What are Angular lifecycle hooks (in order)?**

1. **`ngOnChanges`** — when an `@Input` value changes.
2. **`ngOnInit`** — once, after the first `ngOnChanges`. Best place for init logic / API calls.
3. **`ngDoCheck`** — every change detection run. For custom checks.
4. **`ngAfterContentInit`** — once, after projected content (`<ng-content>`) is set.
5. **`ngAfterContentChecked`** — after each check of projected content.
6. **`ngAfterViewInit`** — once, after component's view + child views ready. Access `@ViewChild` here.
7. **`ngAfterViewChecked`** — after each view check.
8. **`ngOnDestroy`** — just before component is destroyed. Clean up subscriptions/timers.

---

**Q29. What is the difference between `constructor` and `ngOnInit`?**

- **Constructor** — runs when Angular creates the class. Use only for **dependency injection**.
- **`ngOnInit`** — runs after `@Input` values are set. Put **initialization logic** (API calls, form setup) here.

```typescript
constructor(private api: ProductService) {} // DI only

ngOnInit() {
  this.api.getAll().subscribe(p => this.products = p); // init logic
}
```

---

**Q30. What does `ngOnChanges` give you?**

Fires whenever an `@Input` value changes. Receives a `SimpleChanges` object with previous and current values.

```typescript
@Input() product!: Product;

ngOnChanges(changes: SimpleChanges) {
  if (changes['product']) {
    console.log('previous:', changes['product'].previousValue);
    console.log('current:',  changes['product'].currentValue);
  }
}
```

---

## Part 5: Templates & Data Binding

---

**Q31. What are the 4 types of data binding in Angular?**

- **Interpolation** `{{ value }}` — component → template (display).
- **Property binding** `[prop]="value"` — component → DOM property.
- **Event binding** `(event)="handler()"` — DOM → component.
- **Two-way binding** `[(ngModel)]="value"` — both directions. Needs `FormsModule`.

---

**Q32. What is interpolation?**

`{{ expression }}` puts a component value into the template. Read-only.

```html
<p>Hello {{ user.name }}, you have {{ messages.length }} messages.</p>
```

Can use expressions but **not** statements (no `=`, no `if`).

---

**Q33. What is property binding?**

Set a DOM property from the component using `[prop]`.

```html
<img [src]="user.photoUrl" [alt]="user.name" />
<button [disabled]="isSaving">Save</button>
```

Difference from `src="..."`: property binding evaluates the right side as TypeScript, attribute binding is a literal string.

---

**Q34. What is event binding?**

Listen to DOM events with `(event)`.

```html
<button (click)="save()">Save</button>
<input (input)="onType($event)" (keyup.enter)="search()" />
```

`$event` is the event object. Angular supports key modifiers like `.enter`, `.escape`.

---

**Q35. What is two-way binding with `[(ngModel)]`?**

Combines property + event binding for inputs. The component and the input stay in sync.

```html
<input [(ngModel)]="user.name" />
<!-- equivalent to: -->
<input [ngModel]="user.name" (ngModelChange)="user.name = $event" />
```

Requires `FormsModule` to be imported.

---

**Q36. What are template reference variables (`#var`)?**

Give an element a name in the template so you can refer to it elsewhere in the same template.

```html
<input #nameInput type="text" />
<button (click)="greet(nameInput.value)">Greet</button>

<input #emailRef ngModel name="email" />
<p>Valid? {{ emailRef.valid }}</p>
```

---

**Q37. What is `ng-container`?**

A grouping element that **renders nothing** in the DOM. Use it to apply `*ngIf` or `*ngFor` without adding an extra `<div>`.

```html
<ng-container *ngIf="user">
  <h2>{{ user.name }}</h2>
  <p>{{ user.email }}</p>
</ng-container>
```

---

**Q38. What is `ng-template`?**

A block of HTML that is **not rendered by default**. Used by `*ngIf` else, `*ngFor`, and dynamic rendering via `ViewContainerRef`.

```html
<div *ngIf="user; else loading">
  {{ user.name }}
</div>
<ng-template #loading>
  <p>Loading...</p>
</ng-template>
```

---

**Q39. What is `ng-content` (content projection)?**

A placeholder where the parent's content gets inserted into your component. Like `slot` in Web Components.

```html
<!-- card.component.html -->
<div class="card">
  <ng-content></ng-content>
</div>

<!-- parent -->
<app-card>
  <h2>Hello</h2>   <!-- this gets projected into the card -->
</app-card>
```

Multi-slot projection: `<ng-content select="[header]"></ng-content>`.

---

**Q40. What are class and style bindings?**

```html
<!-- single class -->
<div [class.active]="isActive"></div>

<!-- multiple classes via ngClass -->
<div [ngClass]="{ active: isActive, highlight: hasError }"></div>

<!-- single style -->
<div [style.color]="textColor" [style.font-size.px]="size"></div>

<!-- multiple styles via ngStyle -->
<div [ngStyle]="{ color: textColor, 'font-size.px': size }"></div>
```

---

## Part 6: Directives

---

**Q41. What are the types of directives in Angular?**

- **Component** — directive with a template (every `@Component` is technically a directive).
- **Structural** — change DOM structure: `*ngIf`, `*ngFor`, `*ngSwitch`. The `*` is sugar for `<ng-template>`.
- **Attribute** — change appearance/behavior: `ngClass`, `ngStyle`. Or custom ones with `@Directive`.

---

**Q42. How does `*ngIf` work, including the `else` branch?**

Conditionally adds/removes an element from the DOM.

```html
<div *ngIf="user; else loading">Hello {{ user.name }}</div>
<ng-template #loading>Loading...</ng-template>

<div *ngIf="user as u; else loading">
  Hello {{ u.name }} <!-- local alias -->
</div>
```

`*ngIf` removes the element when false (not just hides it).

---

**Q43. How do you use `*ngFor` and what is `trackBy`?**

Repeats an element for each item in an iterable.

```html
<li *ngFor="let item of items; let i = index; trackBy: trackById">
  {{ i + 1 }}. {{ item.name }}
</li>
```

```typescript
trackById(index: number, item: Item) { return item.id; }
```

`trackBy` tells Angular how to identify items so when the list changes, it reuses DOM nodes instead of recreating them. Massive performance win on large lists.

---

**Q44. How does `*ngSwitch` work?**

```html
<div [ngSwitch]="status">
  <p *ngSwitchCase="'loading'">Loading...</p>
  <p *ngSwitchCase="'success'">Done!</p>
  <p *ngSwitchDefault>Unknown state</p>
</div>
```

---

**Q45. What is the difference between `ngClass` and `ngStyle`?**

- **`ngClass`** — apply CSS classes conditionally.
- **`ngStyle`** — apply inline CSS styles conditionally.

```html
<div [ngClass]="{ 'is-active': active, 'has-error': error }"></div>
<div [ngStyle]="{ color: status === 'error' ? 'red' : 'green' }"></div>
```

---

**Q46. How do you create a custom attribute directive?**

```typescript
@Directive({ selector: '[appHighlight]', standalone: true })
export class HighlightDirective {
  @Input() appHighlight = 'yellow';

  constructor(private el: ElementRef) {}

  @HostListener('mouseenter') onEnter() {
    this.el.nativeElement.style.backgroundColor = this.appHighlight;
  }
  @HostListener('mouseleave') onLeave() {
    this.el.nativeElement.style.backgroundColor = '';
  }
}
```

```html
<p appHighlight="lightblue">Hover me</p>
```

---

**Q47. What are `@HostListener` and `@HostBinding`?**

- **`@HostListener`** — listen to events on the host element.
- **`@HostBinding`** — bind a property of the host element (like adding a class).

```typescript
@HostBinding('class.active') isActive = false;

@HostListener('click') toggle() { this.isActive = !this.isActive; }
```

---

## Part 7: Pipes

---

**Q48. What are pipes in Angular?**

Pipes transform a value for display. Used in templates with `|`.

```html
<p>{{ price | currency:'USD' }}</p>
<p>{{ today | date:'longDate' }}</p>
<p>{{ user | json }}</p>
```

---

**Q49. What are some built-in pipes?**

- `date` — format dates.
- `currency` — format money.
- `decimal` / `number` — format numbers.
- `percent` — show as %.
- `uppercase` / `lowercase` / `titlecase`.
- `json` — pretty-print an object (debugging).
- `slice` — slice arrays/strings.
- `async` — subscribe to Observables/Promises.

---

**Q50. How do you chain pipes?**

Apply multiple pipes left to right.

```html
<p>{{ name | lowercase | slice:0:10 }}</p>
<p>{{ today | date:'shortDate' | uppercase }}</p>
```

---

**Q51. How do you create a custom pipe?**

```typescript
@Pipe({ name: 'truncate', standalone: true })
export class TruncatePipe implements PipeTransform {
  transform(value: string, limit = 20): string {
    if (!value) return '';
    return value.length > limit ? value.substring(0, limit) + '…' : value;
  }
}
```

```html
<p>{{ longText | truncate:50 }}</p>
```

---

**Q52. What is the difference between Pure and Impure pipes?**

- **Pure (default)** — runs only when the input reference changes. Fast.
- **Impure** — runs on every change detection cycle. Slow if misused.

The `async` pipe is impure (it must check Observables every cycle). Avoid impure pipes for filtering/sorting big arrays — do that in the component.

```typescript
@Pipe({ name: 'myFilter', pure: false })  // impure
```

---

## Part 8: Component Communication & Lifecycle

---

**Q53. How does `@Input()` work (parent → child)?**

Parent passes data; child receives via `@Input`.

```typescript
// child.component.ts
@Input() title = '';
@Input({ required: true }) product!: Product;
```

```html
<!-- parent template -->
<app-child [title]="myTitle" [product]="selectedProduct"></app-child>
```

---

**Q54. How does `@Output()` and `EventEmitter` work (child → parent)?**

Child emits an event; parent listens.

```typescript
// child.component.ts
@Output() saved = new EventEmitter<string>();

onSave() { this.saved.emit('done'); }
```

```html
<app-child (saved)="onChildSaved($event)"></app-child>
```

---

**Q55. What is `@ViewChild` and when do you use it?**

Get a reference to an element or child component **in this component's template**.

```typescript
@ViewChild('nameInput') input!: ElementRef<HTMLInputElement>;
@ViewChild(ChildComponent) child!: ChildComponent;

ngAfterViewInit() {
  this.input.nativeElement.focus();
  this.child.refresh();
}
```

Access in `ngAfterViewInit` (not earlier — the view isn't ready before that).

---

**Q56. What is the difference between `@ViewChild` and `@ContentChild`?**

- **`@ViewChild`** — element in **this component's own template**.
- **`@ContentChild`** — content **projected into this component** from the parent via `<ng-content>`.

```typescript
@ContentChild(TabTitleComponent) tabTitle!: TabTitleComponent;
ngAfterContentInit() { console.log(this.tabTitle.title); }
```

---

**Q57. How do you handle sibling component communication?**

Use a **shared service** with a `BehaviorSubject`. Both siblings inject the same service.

```typescript
@Injectable({ providedIn: 'root' })
export class CartService {
  private items = new BehaviorSubject<Item[]>([]);
  items$ = this.items.asObservable();
  add(item: Item) { this.items.next([...this.items.value, item]); }
}
```

Sibling A calls `add()`; sibling B subscribes to `items$` and updates automatically.

---

**Q58. When does each lifecycle hook actually fire?**

Quick reference:

- **Input change** → `ngOnChanges`
- **First render** → `ngOnInit` (after first `ngOnChanges`)
- **Every CD cycle** → `ngDoCheck`, `ngAfterContentChecked`, `ngAfterViewChecked`
- **Projected content ready** → `ngAfterContentInit`
- **View ready** → `ngAfterViewInit` (now you can access `@ViewChild`)
- **Component destroyed** → `ngOnDestroy` (unsubscribe, clear timers)

---

## Part 9: Forms

---

**Q59. What is the difference between Template-Driven and Reactive forms?**

| Feature | Template-Driven | Reactive |
|---|---|---|
| Logic in | HTML template | Component class |
| Setup | `ngModel`, `FormsModule` | `FormGroup`, `ReactiveFormsModule` |
| Testing | Harder | Easy (pure class) |
| Dynamic fields | Cumbersome | Easy (`FormArray`) |
| Validation | HTML attributes | Validators in code |
| Use for | Simple forms | Complex / dynamic / testable forms |

---

**Q60. What are `FormGroup`, `FormControl`, and `FormArray`?**

Building blocks of Reactive forms.

- **`FormControl`** — one input field.
- **`FormGroup`** — group of named controls (an object).
- **`FormArray`** — list of controls (an array — for dynamic fields).

```typescript
form = new FormGroup({
  name: new FormControl(''),
  emails: new FormArray([ new FormControl('') ])
});
```

---

**Q61. What is `FormBuilder`?**

A helper service that makes creating forms less verbose.

```typescript
constructor(private fb: FormBuilder) {}

ngOnInit() {
  this.form = this.fb.group({
    name: ['', Validators.required],
    age: [0, [Validators.min(0), Validators.max(120)]],
    address: this.fb.group({
      city: [''], country: ['']
    })
  });
}
```

---

**Q62. What are the built-in validators?**

From the `Validators` class: `required`, `requiredTrue`, `minLength`, `maxLength`, `min`, `max`, `email`, `pattern`, `nullValidator`.

```typescript
this.form = this.fb.group({
  email: ['', [Validators.required, Validators.email]],
  password: ['', [Validators.required, Validators.minLength(8)]]
});
```

```html
<div *ngIf="form.get('email')?.hasError('email')">Invalid email</div>
```

---

**Q63. How do you create a custom validator?**

A function that returns an error object or `null`.

```typescript
function noSpaces(control: AbstractControl): ValidationErrors | null {
  return /\s/.test(control.value) ? { noSpaces: true } : null;
}

this.form = this.fb.group({
  username: ['', [Validators.required, noSpaces]]
});
```

For cross-field validation (password match), apply the validator at the `FormGroup` level.

---

**Q64. How do you submit a Reactive form?**

```html
<form [formGroup]="form" (ngSubmit)="onSubmit()">
  <input formControlName="email" />
  <button type="submit" [disabled]="form.invalid">Save</button>
</form>
```

```typescript
onSubmit() {
  if (this.form.invalid) return;
  this.api.save(this.form.value).subscribe();
  this.form.reset();
}
```

---

**Q65. What are the FormControl status flags?**

Useful for showing validation messages at the right time.

- **`valid` / `invalid`** — passes/fails validators.
- **`touched` / `untouched`** — user has/hasn't blurred the field.
- **`dirty` / `pristine`** — value has/hasn't been changed.
- **`pending`** — async validator running.

```html
<div *ngIf="form.get('email')?.invalid && form.get('email')?.touched">
  Please enter a valid email.
</div>
```

---

## Part 10: Routing

---

**Q66. How do you set up routing in Angular?**

Define a route map and add it to the router.

```typescript
const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'products', component: ProductListComponent },
  { path: 'products/:id', component: ProductDetailComponent },
  { path: '**', component: NotFoundComponent }   // wildcard last
];

@NgModule({ imports: [RouterModule.forRoot(routes)], exports: [RouterModule] })
export class AppRoutingModule { }
```

---

**Q67. What are `router-outlet` and `routerLink`?**

- **`<router-outlet>`** — placeholder where the routed component renders.
- **`[routerLink]`** — navigation link (instead of `href`).

```html
<nav>
  <a routerLink="/">Home</a>
  <a [routerLink]="['/products', product.id]">Details</a>
</nav>
<router-outlet></router-outlet>
```

`routerLinkActive="active"` adds a class to the currently active link.

---

**Q68. How do you read route parameters (`:id`)?**

```typescript
constructor(private route: ActivatedRoute) {}

ngOnInit() {
  // snapshot — doesn't update if user navigates within the same component
  const id = this.route.snapshot.params['id'];

  // observable — updates on every navigation
  this.route.params.subscribe(p => this.loadProduct(p['id']));

  // observable with paramMap (preferred)
  this.route.paramMap.subscribe(p => this.loadProduct(p.get('id')!));
}
```

---

**Q69. How do you read query parameters (`?page=2`)?**

```typescript
this.route.queryParams.subscribe(q => {
  this.page = +q['page'] || 1;
});

// Navigate with query params
this.router.navigate(['/products'], { queryParams: { page: 2 } });
```

---

**Q70. How do you set up nested (child) routes?**

```typescript
const routes: Routes = [
  {
    path: 'admin',
    component: AdminLayoutComponent,
    children: [
      { path: 'users',    component: UsersComponent },
      { path: 'settings', component: SettingsComponent }
    ]
  }
];
```

The parent component needs its own `<router-outlet>` for child routes to render inside it.

---

**Q71. What is lazy loading and how do you set it up?**

A feature module loads only when the user navigates to it. Reduces initial bundle size.

```typescript
const routes: Routes = [
  {
    path: 'admin',
    loadChildren: () => import('./admin/admin.module').then(m => m.AdminModule)
  }
];

// Or with standalone components:
{
  path: 'product/:id',
  loadComponent: () => import('./product.component').then(m => m.ProductComponent)
}
```

---

**Q72. What are route guards and the types?**

Guards decide whether navigation is allowed.

- **`CanActivate`** — can the user enter this route? (auth check)
- **`CanActivateChild`** — same for children.
- **`CanDeactivate`** — can the user leave? (unsaved changes warning)
- **`CanLoad`** / **`CanMatch`** — should the lazy module load at all?
- **`Resolve`** — prefetch data before activation.

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

**Q73. What is a Resolver and when do you use it?**

Pre-fetches data before the route activates, so the component has the data ready in `ngOnInit`.

```typescript
@Injectable({ providedIn: 'root' })
export class ProductResolver implements Resolve<Product> {
  constructor(private api: ProductService) {}
  resolve(route: ActivatedRouteSnapshot): Observable<Product> {
    return this.api.getById(+route.params['id']);
  }
}

// route
{ path: 'products/:id', component: DetailComponent, resolve: { product: ProductResolver } }

// component
this.route.data.subscribe(d => this.product = d['product']);
```

---

## Part 11: Services & Dependency Injection

---

**Q74. What is a service in Angular?**

A class that holds reusable business logic, data, or state — separate from components. Typically used for API calls, shared state, or utilities.

```typescript
@Injectable({ providedIn: 'root' })
export class ProductService {
  constructor(private http: HttpClient) {}
  getAll() { return this.http.get<Product[]>('/api/products'); }
}
```

---

**Q75. What is Dependency Injection in Angular?**

A pattern where a class receives its dependencies from outside, instead of creating them itself. Angular has a built-in hierarchical injector — when it sees a type in a constructor, it provides an instance.

```typescript
// Angular injects ProductService automatically
constructor(private products: ProductService) {}
```

You don't `new` services — Angular does it for you.

---

**Q76. What does `providedIn: 'root'` mean?**

The service is a **singleton** for the entire app — one shared instance, automatically tree-shaken if unused.

```typescript
@Injectable({ providedIn: 'root' })
export class AuthService { }
```

This is the default and preferred way for most services.

---

**Q77. How are services scoped?**

- **`providedIn: 'root'`** — single instance app-wide.
- **`providedIn: SomeModule`** — single instance for that module.
- **In a component's `providers`** — new instance per component (and its children).
- **`providedIn: 'platform'`** — shared across multiple Angular apps on the page.

Component-level scope is useful when each component needs its own state.

---

**Q78. What is the hierarchical injector?**

Angular has a tree of injectors that mirrors the component tree. When a service is requested, Angular walks **up** the tree looking for a provider. If a component declares its own provider, it gets a new instance; otherwise it shares the parent's.

This lets you override services for specific component subtrees.

---

## Part 12: HTTP Client

---

**Q79. How do you set up `HttpClient`?**

Import `HttpClientModule` in `AppModule` (or provide `provideHttpClient()` for standalone apps), then inject `HttpClient`.

```typescript
// AppModule
imports: [HttpClientModule]

// Standalone (main.ts)
bootstrapApplication(AppComponent, {
  providers: [provideHttpClient()]
});

// Service
constructor(private http: HttpClient) {}
```

---

**Q80. How do you make HTTP GET / POST / PUT / DELETE calls?**

```typescript
this.http.get<Product[]>('/api/products');
this.http.get<Product>(`/api/products/${id}`);
this.http.post<Product>('/api/products', product);
this.http.put<Product>(`/api/products/${id}`, product);
this.http.delete<void>(`/api/products/${id}`);
```

All return **Observables** — subscribe to trigger the request.

---

**Q81. How do you send HTTP headers and query parameters?**

```typescript
const headers = new HttpHeaders({
  'Authorization': 'Bearer ' + token,
  'Content-Type': 'application/json'
});

const params = new HttpParams()
  .set('page', '1')
  .set('size', '20');

this.http.get('/api/products', { headers, params });
```

---

**Q82. What are HTTP Interceptors and what do you use them for?**

Middleware that runs on every HTTP request/response. Common uses:
- Attach the auth token to every request.
- Show/hide a global loading spinner.
- Catch 401 → redirect to login.
- Log requests, retry failures.

```typescript
@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private auth: AuthService) {}
  intercept(req: HttpRequest<any>, next: HttpHandler) {
    const token = this.auth.getToken();
    const authReq = token
      ? req.clone({ setHeaders: { Authorization: `Bearer ${token}` } })
      : req;
    return next.handle(authReq);
  }
}
```

Register: `{ provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }`.

---

**Q83. How do you handle HTTP errors globally?**

Use an interceptor with `catchError`.

```typescript
intercept(req: HttpRequest<any>, next: HttpHandler) {
  return next.handle(req).pipe(
    catchError((error: HttpErrorResponse) => {
      if (error.status === 401) this.router.navigate(['/login']);
      else if (error.status === 403) this.router.navigate(['/forbidden']);
      else this.toast.error('Something went wrong');
      return throwError(() => error);
    })
  );
}
```

---

## Part 13: RxJS & Observables

---

**Q84. What is the difference between a Promise and an Observable?**

| Feature | Promise | Observable |
|---|---|---|
| Values | One | Stream of 0..many |
| Execution | Eager (runs now) | Lazy (runs on subscribe) |
| Cancellable | No | Yes (unsubscribe) |
| Operators | Limited | Full RxJS pipeline |

Angular uses Observables for HTTP, Router, Forms, EventEmitter.

---

**Q85. How do you subscribe and unsubscribe?**

```typescript
ngOnInit() {
  this.sub = this.service.getData().subscribe(data => this.data = data);
}

ngOnDestroy() {
  this.sub.unsubscribe();
}
```

Forgetting to unsubscribe = memory leak. Prefer the `async` pipe or the `takeUntil` pattern.

---

**Q86. What are the most useful RxJS operators?**

- **`map`** — transform each value.
- **`filter`** — keep matching values.
- **`tap`** — side effects (logging) without changing the stream.
- **`debounceTime`** — wait for a pause before emitting (good for search inputs).
- **`distinctUntilChanged`** — skip if value didn't change.
- **`switchMap`** — switch to a new Observable, cancelling the previous.
- **`mergeMap`** — run multiple inner Observables in parallel.
- **`concatMap`** — queue inner Observables, run in order.
- **`catchError`** — handle errors with a fallback.
- **`takeUntil`** — complete when another Observable emits.
- **`forkJoin`** — wait for all to complete, emit results together.
- **`combineLatest`** — emit whenever any source emits.

---

**Q87. What's the difference between `Subject`, `BehaviorSubject`, `ReplaySubject`, and `AsyncSubject`?**

All are Observables you can also push to.

- **`Subject`** — no initial value, no memory. New subscribers only get future values.
- **`BehaviorSubject`** — holds the **current value**. New subscribers immediately get it.
- **`ReplaySubject(n)`** — replays last `n` emissions to new subscribers.
- **`AsyncSubject`** — emits only the **last** value, only on complete.

`BehaviorSubject` is the most useful for shared state.

---

**Q88. What's the difference between `switchMap`, `mergeMap`, `concatMap`, and `exhaustMap`?**

All transform each value into a new Observable.

- **`switchMap`** — cancel the previous, switch to the new. Good for search-as-you-type.
- **`mergeMap`** — run all in parallel.
- **`concatMap`** — queue and run in order. Good for sequential saves.
- **`exhaustMap`** — ignore new emissions while one is in flight. Good for login button (prevent double-submit).

---

**Q89. How do you prevent memory leaks from subscriptions using `takeUntil`?**

```typescript
private destroy$ = new Subject<void>();

ngOnInit() {
  this.service.getData()
    .pipe(takeUntil(this.destroy$))
    .subscribe(data => this.data = data);
}

ngOnDestroy() {
  this.destroy$.next();
  this.destroy$.complete();
}
```

Now every subscription using `takeUntil(this.destroy$)` is cleaned up automatically.

---

**Q90. How do you implement search-as-you-type with RxJS?**

```typescript
searchControl = new FormControl('');

results$ = this.searchControl.valueChanges.pipe(
  debounceTime(300),                  // wait for pause
  distinctUntilChanged(),             // skip duplicates
  filter(term => (term ?? '').length > 2),
  switchMap(term => this.api.search(term!).pipe(catchError(() => of([]))))
);
```

```html
<input [formControl]="searchControl" />
<div *ngFor="let r of results$ | async">{{ r.name }}</div>
```

---

**Q91. What is the `async` pipe and why prefer it?**

It subscribes to an Observable/Promise in the template and **auto-unsubscribes** when the component is destroyed.

```typescript
products$ = this.api.getAll();
```

```html
<div *ngFor="let p of products$ | async">{{ p.name }}</div>
```

No `ngOnDestroy` cleanup needed. Best default for any template subscription.

---

## Part 14: Performance & Optimization

---

**Q92. How does Angular's Change Detection work?**

Angular checks every component on every async event (click, HTTP response, timer) to see what changed and update the DOM. **Zone.js** patches async APIs and tells Angular "something happened, check now."

By default the whole component tree is checked top-down on every event. This is fast for small apps but can hurt large ones.

---

**Q93. What is `ChangeDetectionStrategy.OnPush`?**

With `OnPush`, Angular **skips** the component during change detection unless:
- An `@Input` got a **new reference** (not a mutated object).
- An event fired inside the component.
- An Observable bound with `async` emits.
- You call `markForCheck()` manually.

Use it on display-only components (cards, list items) to massively cut CD work.

```typescript
@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  // ...
})
```

---

**Q94. What is the difference between AOT and JIT compilation?**

- **JIT (Just-In-Time)** — templates compiled in the browser at runtime. Slower startup, bigger bundle. Used in dev.
- **AOT (Ahead-Of-Time)** — templates compiled at build time. Faster rendering, smaller bundles, template errors caught at build time. Default in production builds.

---

**Q95. What is Tree Shaking?**

The build step that removes unused code from the final bundle. Angular CLI (Webpack/esbuild) analyzes imports and drops everything not actually used.

Tip: import only what you need (`import { map } from 'rxjs'`) instead of the whole library.

---

## Part 15: State Management

---

**Q96. How do you share state between unrelated components without NgRx?**

Use a shared service with a `BehaviorSubject`. Simple, no extra library, covers most needs.

```typescript
@Injectable({ providedIn: 'root' })
export class CartService {
  private items = new BehaviorSubject<Item[]>([]);
  items$ = this.items.asObservable();

  add(item: Item) {
    this.items.next([...this.items.value, item]);
  }
}
```

Any component that injects `CartService` and subscribes to `items$` gets live updates.

---

**Q97. What is NgRx and when should you use it?**

A Redux-style state management library for Angular: one immutable global **store**.

Core pieces:
- **Store** — single source of truth.
- **Action** — describes what happened (`[Cart] Add Item`).
- **Reducer** — pure function: `(state, action) → newState`.
- **Selector** — read a slice of state.
- **Effect** — handle side effects (API calls) triggered by actions.

Use NgRx for large apps with lots of shared state and debugging needs (Redux DevTools time-travel). For small/medium apps, a service with `BehaviorSubject` is enough.

---

## Part 16: Security, Build & Deployment

---

**Q98. How does Angular protect against XSS, and what about CSRF?**

**XSS:** Angular automatically sanitizes all values bound via interpolation `{{ }}` and property bindings. Dangerous HTML is escaped. Use `DomSanitizer.bypassSecurityTrustHtml()` only for HTML you fully control — never user input.

**CSRF:** Angular's `HttpClient` reads the `XSRF-TOKEN` cookie and sends it as `X-XSRF-TOKEN` header on non-GET requests. The server validates it. With stateless JWT auth (no cookies), CSRF isn't a concern.

---

**Q99. How do you handle different environments (dev / prod) and build for production?**

Angular CLI uses **file replacement** based on `angular.json`.

```typescript
// src/environments/environment.ts (dev)
export const environment = { production: false, apiUrl: 'http://localhost:5000' };

// src/environments/environment.prod.ts
export const environment = { production: true, apiUrl: 'https://api.example.com' };
```

```typescript
import { environment } from '../environments/environment';
this.http.get(`${environment.apiUrl}/products`);
```

Build commands:
```bash
ng build                                  # dev build
ng build --configuration production       # production build (AOT + minified + uglified)
```

Output goes to `dist/` — deploy to App Service, Azure Static Web Apps, S3, Nginx, etc.

---

**Q100. How do you keep an Angular app up to date through major versions?**

Angular releases a major version every 6 months with 18 months LTS.

1. Check **update.angular.io** for a step-by-step guide between your current and target version.
2. **Upgrade one major version at a time** (v16 → v17, not v15 → v18).
3. Run `ng update @angular/core @angular/cli` — updates packages and applies schematics that auto-fix breaking changes.
4. Fix remaining TS errors, run your test suite.
5. Update third-party libraries (Material, NgRx) **after** the core update.
6. Pay attention to deprecation warnings — they appear 1–2 majors before removal.

---

