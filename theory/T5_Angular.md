# Angular & TypeScript — Interview Prep Lookup
> For .NET Full Stack Developers | 20 TypeScript + 80 Angular Questions

---

# PART 1 — TYPESCRIPT BASICS (20 Questions)

---

## 🔷 Q1. What is TypeScript and why does Angular use it?

TypeScript is JavaScript with static type checking compiled by `tsc` to plain JS. Angular is written in TypeScript and requires it.

**Benefits:**
- Catches errors at **compile time**, not runtime
- IDE autocomplete, go-to-definition, safe refactoring
- Self-documenting — types describe intent (like C# XML docs)
- Clean OOP: interfaces, generics, access modifiers, decorators

> 💡 **For .NET devs:** TypeScript feels like C# for the browser — same OOP patterns, same type safety mindset.

---

## 🔷 Q2. What are all types in TypeScript?

## 1. Primitive Types
```typescript
let name: string = 'Purushottam';
let age: number = 25;
let isActive: boolean = true;
let bigNum: bigint = 9007199254740991n;
let sym: symbol = Symbol('id');
```
 
## 2. Special Types
```typescript
let a: any = 'anything';        // disables type check (avoid)
let b: unknown = getData();     // safe any — must narrow before use
let c: null = null;             // intentionally empty
let d: undefined = undefined;   // not yet assigned
 
function log(): void { }        // returns nothing
function fail(): never { throw new Error(); }  // never returns
```
 
## 3. Complex Types
```typescript
let ids: number[] = [1, 2, 3];                    // array
let point: [number, number] = [10, 20];            // tuple
let user: { id: number; name: string } = { ... }; // object
enum Role { Admin = 'ADMIN', User = 'USER' }       // enum
```

---

## 🔷 Q3. What are `any`, `unknown`, `void`, and `never`?

```typescript
// any — disables type checking (AVOID)
let data: any = 'hello';
data = 42;                  // no error, no safety

// unknown — safe version of any (PREFER)
let input: unknown = getUserInput();
if (typeof input === 'string') {
  input.toUpperCase();      // ✅ must narrow first
}

// void — function returns nothing
ngOnInit(): void { ... }

// never — function never completes (throws or infinite loop)
function throwError(msg: string): never {
  throw new Error(msg);
}
```

| Type | Safe? | When to Use |
|---|---|---|
| `any` | ❌ | Migrating JS to TS only |
| `unknown` | ✅ | Type not yet known |
| `void` | ✅ | Function with no return |
| `never` | ✅ | Unreachable code / throws |

---

## 🔷 Q4. What is the difference between `null` and `undefined`?

```typescript
let a: undefined = undefined;   // declared, never assigned (JS sets this)
let b: null = null;             // intentionally empty (you set this)

// Union pattern — common in Angular
let user: User | null = null;
let token: string | undefined;

// Nullish coalescing — use default if null/undefined
let username = user?.name ?? 'Guest';
```

`null == undefined` → `true` (loose)
`null === undefined` → `false` (strict)

---

## 🔷 Q5. How do you type arrays and tuples?

```typescript
// Arrays — two equivalent syntaxes
let ids: number[] = [1, 2, 3];
let names: Array<string> = ['Alice', 'Bob'];

// Mixed types
let mixed: (string | number)[] = [1, 'two', 3];

// Readonly array — no push/pop/mutate
let fixed: readonly number[] = [1, 2, 3];
fixed.push(4);   // ❌ Error

// Tuple — fixed length, each position typed
let point: [number, number] = [10, 20];
let entry: [name: string, age: number] = ['Purushottam', 25];

// Destructure tuple
const [x, y] = point;
const [name, age] = entry;
```

---

## 🔷 Q6. What is the difference between `interface` and `type`?

```typescript
// Interface — preferred for object shapes, can be extended and merged
interface User {
  id: number;
  name: string;
  email?: string;    // optional
}
interface AdminUser extends User {
  role: string;
}

// Type — for unions, primitives, intersections, tuples
type Status = 'active' | 'inactive' | 'pending';
type ID = string | number;
type Point = [number, number];
```

| Feature | `interface` | `type` |
|---|---|---|
| Extend | `extends` | `&` intersection |
| Merge declarations | ✅ Yes | ❌ No |
| Unions / Tuples | ❌ | ✅ |
| Preferred for | Object shapes | Everything else |

---

## 🔷 Q7. What are Union and Intersection types?

```typescript
// Union — one OR another
type Input = string | number;

function format(val: string | number): string {
  return val.toString();
}

// Literal union — only specific values
type Direction = 'up' | 'down' | 'left' | 'right';
type LoadingState = 'idle' | 'loading' | 'success' | 'error';

// Intersection — must satisfy ALL types
interface HasName { name: string; }
interface HasAge  { age: number; }
type Person = HasName & HasAge;

const p: Person = { name: 'Purushottam', age: 25 };
```

---

## 🔷 Q8. What is an `enum`?

```typescript
// Numeric enum (default)
enum Direction { Up, Down, Left, Right }  // 0, 1, 2, 3

// String enum — preferred (readable in logs and API responses)
enum UserRole {
  Admin  = 'ADMIN',
  Editor = 'EDITOR',
  Viewer = 'VIEWER'
}

// Usage in Angular component
role: UserRole = UserRole.Admin;
if (this.role === UserRole.Admin) { ... }
```

> 💡 Use **string enums** — easier to debug and read in API JSON.

---

## 🔷 Q9. What are access modifiers in TypeScript classes?

```typescript
export class UserComponent {
  public username: string = '';      // accessible everywhere (default)
  private apiUrl: string = '/api';   // only inside this class
  protected userId: number = 0;      // this class + subclasses
  readonly MAX: number = 100;        // can't be reassigned after init

  // Constructor shorthand — declares AND assigns in one line
  constructor(
    private userService: UserService,    // ← Angular DI pattern
    public router: Router
  ) {}
}
```

> 💡 **For .NET devs:** Same as C# access modifiers. `private` in constructor is Angular's standard DI pattern.

---

## 🔷 Q10. What are Generics in TypeScript?

```typescript
// Generic function — works with any type, stays type-safe
function getFirst<T>(arr: T[]): T {
  return arr[0];
}

getFirst<string>(['a', 'b']);    // returns string
getFirst<number>([1, 2, 3]);    // returns number

// Generic with constraint
function getProperty<T, K extends keyof T>(obj: T, key: K): T[K] {
  return obj[key];
}

// Angular usage — you see <T> everywhere:
http.get<User[]>('/api/users')          // HttpClient
new EventEmitter<string>()              // @Output
Observable<Product[]>                   // RxJS
signal<number>(0)                       // Angular Signals
```

---

## 🔷 Q11. What is Type Narrowing?

Narrowing means refining a broad type to a specific one inside a condition.

```typescript
function display(value: string | number) {
  if (typeof value === 'string') {
    console.log(value.toUpperCase());   // TypeScript knows: string
  } else {
    console.log(value.toFixed(2));      // TypeScript knows: number
  }
}

// With objects — use 'in' operator
interface Circle { radius: number; }
interface Square { side: number; }

function area(shape: Circle | Square): number {
  if ('radius' in shape) {
    return Math.PI * shape.radius ** 2;   // Circle
  }
  return shape.side ** 2;               // Square
}
```

---

## 🔷 Q12. What are Optional Chaining `?.` and Non-Null Assertion `!`?

```typescript
// Optional chaining — returns undefined if anything in chain is null/undefined
const city = user?.address?.city;
const len = user?.name?.length;
user?.save();                              // safe method call

// Non-null assertion — "trust me, not null" (no runtime check)
const el = document.getElementById('app')!;

// Angular usage
@ViewChild('nameInput') nameInput!: ElementRef;   // ← very common
@Input({ required: true }) product!: Product;
```

> ⚠️ Prefer `?.` over `!`. Only use `!` when you're certain the value exists (e.g. after `ngAfterViewInit`).

---

## 🔷 Q13. What are Utility Types?

Built-in TypeScript helpers that transform existing types.

```typescript
interface User {
  id: number;
  name: string;
  email: string;
  password: string;
}

Partial<User>          // all props optional — great for PATCH / edit forms
Required<User>         // all props required
Readonly<User>         // nothing can be mutated
Pick<User, 'id'|'name'>       // select only these props
Omit<User, 'password'>        // exclude these props
Record<string, number>        // key-value map: { [key: string]: number }
ReturnType<typeof myFn>       // infer a function's return type
```

```typescript
// Real Angular use cases
function patchUser(changes: Partial<User>) { ... }         // PATCH body
type PublicUser = Omit<User, 'password'>;                  // API response
const roleMap: Record<string, UserRole> = { u1: UserRole.Admin };
```

---

## 🔷 Q14. What are Decorators in TypeScript?

Functions prefixed with `@` that attach metadata to classes, methods, or properties.

```typescript
@Component({
  selector: 'app-user',
  templateUrl: './user.component.html'
})
export class UserComponent {

  @Input() userId!: number;
  @Output() selected = new EventEmitter<number>();
  @ViewChild('nameInput') nameInput!: ElementRef;

  constructor(
    @Inject(API_URL) private apiUrl: string,   // @Inject for token-based DI
    private userService: UserService
  ) {}
}
```

Common Angular decorators: `@Component`, `@Directive`, `@Pipe`, `@Injectable`, `@NgModule`, `@Input`, `@Output`, `@ViewChild`, `@HostListener`, `@HostBinding`.

---

## 🔷 Q15. What is Type Inference — where should you annotate vs let TypeScript infer?

```typescript
// ✅ Let TypeScript infer — obvious from value
let count = 0;             // inferred: number
let name = 'Angular';      // inferred: string
let items = [1, 2, 3];    // inferred: number[]

// ✅ Always annotate — function boundaries and class properties
getUserById(id: number): Observable<User> { ... }
users: User[] = [];
isLoading: boolean = false;

// ✅ Always annotate — when initializing to null/undefined
let currentUser: User | null = null;
let selectedId: number | undefined;
```

> 💡 **Rule:** Annotate at **boundaries** (params, return types, class props). Trust inference for local variables.

---

## 🔷 Q16. What are async/await and Promises in TypeScript?

```typescript
// Promise — a future value
function fetchUser(id: number): Promise<User> {
  return fetch(`/api/users/${id}`).then(r => r.json());
}

// async/await — same thing, reads top-to-bottom (like C# async/await)
async function loadUser(id: number): Promise<User> {
  try {
    const response = await fetch(`/api/users/${id}`);
    return await response.json() as User;
  } catch (error) {
    throw new Error('Failed to load user');
  }
}

// In Angular — mostly use Observables, but async/await works with firstValueFrom
const user = await firstValueFrom(this.userService.getById(id));
```

---

## 🔷 Q17. What is destructuring, spread, and rest in TypeScript?

```typescript
// Object destructuring
const { name, age, email = 'default@mail.com' } = user;

// Array destructuring
const [first, second, ...rest] = items;

// Rename while destructuring
const { name: userName, id: userId } = user;

// Spread — expand arrays/objects
const updatedUser = { ...user, name: 'New Name' };
const combined = [...arr1, ...arr2];

// Rest — collect remaining
function logAll(first: string, ...others: string[]): void {
  console.log(first, others);
}

// Typed destructuring in function params
function greet({ name, age }: { name: string; age: number }): string {
  return `${name} is ${age}`;
}
```

---

## 🔷 Q18. What are arrow functions vs regular functions in TypeScript?

```typescript
// Regular function — has its own `this`, can be constructor
function greet(name: string): string {
  return `Hello, ${name}`;
}

// Arrow function — inherits `this` from surrounding scope
const greet = (name: string): string => `Hello, ${name}`;

// Critical in Angular — arrow functions keep component's `this`
export class ProductComponent {
  products: Product[] = [];

  // ✅ Arrow: `this` is the component
  loadProducts = () => {
    this.productService.getAll().subscribe(p => this.products = p);
  }

  // ⚠️ Regular function in callbacks can lose `this`
}
```

---

## 🔷 Q19. What is the `keyof` operator and mapped types?

```typescript
interface User { id: number; name: string; email: string; }

// keyof — union of property names
type UserKey = keyof User;    // 'id' | 'name' | 'email'

// Index access type
type IdType = User['id'];     // number

// Use in functions
function getField<T, K extends keyof T>(obj: T, key: K): T[K] {
  return obj[key];
}

// Mapped type — transform every property
type Optional<T> = {
  [K in keyof T]?: T[K];     // same as Partial<T>
};

type Flags<T> = {
  [K in keyof T]: boolean;   // all props become boolean
};
```

---

## 🔷 Q20. What are Type Guards and `instanceof`?

```typescript
// typeof guard — for primitives
function process(val: string | number) {
  if (typeof val === 'string') { val.toUpperCase(); }
  else { val.toFixed(2); }
}

// instanceof guard — for classes
function handleError(err: Error | HttpErrorResponse) {
  if (err instanceof HttpErrorResponse) {
    console.log(err.status);
  } else {
    console.log(err.message);
  }
}

// Custom type guard — using 'is'
function isUser(obj: any): obj is User {
  return typeof obj.id === 'number' && typeof obj.name === 'string';
}

if (isUser(data)) {
  console.log(data.name);   // TypeScript knows: User
}
```

---
---

# PART 2 — ANGULAR (80 Questions)

---

## 📦 SECTION 1: Angular Fundamentals

---

### Q21. What is Angular?

A TypeScript-based front-end framework by Google for building Single-Page Applications (SPAs). Provides components, routing, forms, HTTP client, dependency injection, and tooling out of the box.

> 💡 **For .NET devs:** Angular is to the browser what ASP.NET MVC is to the server — a full opinionated framework, not just a library.

---

### Q22. What is the difference between Angular and AngularJS?

| Feature | AngularJS (v1) | Angular (v2+) |
|---|---|---|
| Language | JavaScript | TypeScript |
| Architecture | MVC with `$scope` | Component-based |
| Data flow | Two-way digest cycle | Unidirectional + opt-in two-way |
| Performance | Slower | Ivy renderer, tree shaking |
| Tooling | None | Angular CLI |
| Mobile | Limited | Mobile-first |

Not backward compatible — Angular 2+ was a complete rewrite.

---

### Q23. What is the Angular CLI and what are the key commands?

```bash
ng new my-app                     # create project
ng serve                          # dev server → localhost:4200
ng generate component product     # or: ng g c product
ng generate service user          # ng g s user
ng generate module admin          # ng g m admin
ng build                          # dev build
ng build --configuration production   # prod build (AOT + minified)
ng test                           # run unit tests
ng lint                           # lint code
ng update @angular/core @angular/cli  # upgrade Angular version
```

---

### Q24. What is the Angular project structure?

```
my-app/
├── src/
│   ├── app/
│   │   ├── app.component.ts/html/css   ← root component
│   │   ├── app.module.ts               ← root module
│   │   └── feature/                    ← your feature modules/components
│   ├── assets/                         ← images, static files
│   ├── environments/                   ← dev/prod config
│   ├── index.html                      ← shell HTML (<app-root>)
│   ├── main.ts                         ← bootstrap entry point
│   └── styles.css                      ← global styles
├── angular.json                        ← CLI config
├── tsconfig.json                       ← TypeScript config
└── package.json
```

---

### Q25. What does `main.ts` do?

It's the entry point that bootstraps the root module or standalone component.

```typescript
// NgModule-based (Angular < 17)
platformBrowserDynamic().bootstrapModule(AppModule);

// Standalone (Angular 17+, default)
bootstrapApplication(AppComponent, {
  providers: [
    provideRouter(routes),
    provideHttpClient()
  ]
});
```

`index.html` has `<app-root></app-root>` — Angular replaces it with `AppComponent`.

---

### Q26. What is `NgModule` and what does `AppModule` contain?

An `NgModule` groups related components, directives, pipes, and services.

```typescript
@NgModule({
  declarations: [AppComponent, ProductCardComponent],   // components/directives/pipes
  imports: [BrowserModule, FormsModule, HttpClientModule, RouterModule],
  providers: [ProductService],
  exports: [ProductCardComponent],   // make available to other modules
  bootstrap: [AppComponent]          // root only
})
export class AppModule { }
```

---

### Q27. What are standalone components (Angular 17+)?

Components that declare their own imports — no `NgModule` needed. Default in Angular 17+.

```typescript
@Component({
  standalone: true,
  selector: 'app-product-card',
  imports: [CommonModule, RouterModule, AsyncPipe],
  template: `<a [routerLink]="['/products', product.id]">{{ product.name }}</a>`
})
export class ProductCardComponent {
  @Input() product!: Product;
}
```

---

## 🧩 SECTION 2: Components

---

### Q28. What is a component in Angular?

A self-contained piece of UI — a TypeScript class + HTML template + CSS styles. Everything visible in an Angular app is a component.

```typescript
@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class ProductComponent implements OnInit {
  products: Product[] = [];

  constructor(private productService: ProductService) {}

  ngOnInit(): void {
    this.productService.getAll().subscribe(p => this.products = p);
  }
}
```

---

### Q29. What are the different types of component selectors?

```typescript
selector: 'app-product'       // Element:    <app-product>
selector: '[appProduct]'      // Attribute:  <div appProduct>
selector: '.app-product'      // Class:      <div class="app-product">
```

Element selectors are most common for components. Attribute selectors are typical for directives.

---

### Q30. What is View Encapsulation?

Controls how a component's CSS is scoped.

| Mode | Behavior |
|---|---|
| `Emulated` (default) | Angular adds unique attributes; styles scoped to component |
| `None` | Styles become global — leaks app-wide |
| `ShadowDom` | Uses native browser Shadow DOM |

```typescript
@Component({
  encapsulation: ViewEncapsulation.None  // global styles
})
```

---

### Q31. What are Angular lifecycle hooks in order?

```
1. ngOnChanges       — @Input changed (fires BEFORE ngOnInit on first run)
2. ngOnInit          — once after first ngOnChanges; put init logic here
3. ngDoCheck         — every change detection cycle
4. ngAfterContentInit    — once after <ng-content> projected
5. ngAfterContentChecked — after each content check
6. ngAfterViewInit   — once after view + child views ready (@ViewChild available)
7. ngAfterViewChecked    — after each view check
8. ngOnDestroy       — cleanup: unsubscribe, clear timers
```

---

### Q32. What is the difference between `constructor` and `ngOnInit`?

```typescript
// Constructor — only for dependency injection
constructor(private userService: UserService) {}

// ngOnInit — init logic: API calls, form setup (runs after @Input values set)
ngOnInit(): void {
  this.userService.getAll().subscribe(u => this.users = u);
}
```

> 💡 **Rule:** Constructor = DI only. ngOnInit = everything else.

---

### Q33. What does `ngOnChanges` give you?

Fires whenever an `@Input` value changes. Receives a `SimpleChanges` object.

```typescript
@Input() product!: Product;

ngOnChanges(changes: SimpleChanges): void {
  if (changes['product'] && !changes['product'].firstChange) {
    const prev = changes['product'].previousValue;
    const curr = changes['product'].currentValue;
    console.log('Changed from', prev, 'to', curr);
  }
}
```

---

## 📝 SECTION 3: Templates & Data Binding

---

### Q34. What are the 4 types of data binding in Angular?

```html
<!-- 1. Interpolation — component → template (read only) -->
<p>Hello {{ user.name }}</p>

<!-- 2. Property binding — component → DOM property -->
<img [src]="user.photo" [alt]="user.name" />
<button [disabled]="isSaving">Save</button>

<!-- 3. Event binding — DOM → component -->
<button (click)="save()">Save</button>
<input (keyup.enter)="search()" />

<!-- 4. Two-way binding — both directions (needs FormsModule) -->
<input [(ngModel)]="user.name" />
```

---

### Q35. What is the difference between property binding and attribute binding?

```html
<!-- Property binding — evaluates as TypeScript expression -->
<button [disabled]="isLoading">Save</button>

<!-- Attribute binding — for HTML attributes that have no DOM property -->
<td [attr.colspan]="colSpan">Cell</td>
<th [attr.aria-label]="columnName">Header</th>
```

---

### Q36. What are template reference variables?

Give an element a local name in the template.

```html
<input #nameInput type="text" />
<button (click)="greet(nameInput.value)">Greet</button>

<!-- Access Angular directives on the element -->
<input #emailRef="ngModel" [(ngModel)]="email" name="email" required />
<p *ngIf="emailRef.invalid && emailRef.touched">Invalid email</p>
```

---

### Q37. What is `ng-container`, `ng-template`, and `ng-content`?

```html
<!-- ng-container — groups elements without adding a DOM node -->
<ng-container *ngIf="user">
  <h2>{{ user.name }}</h2>
  <p>{{ user.email }}</p>
</ng-container>

<!-- ng-template — not rendered by default; used by *ngIf else etc. -->
<div *ngIf="user; else loading">{{ user.name }}</div>
<ng-template #loading><p>Loading...</p></ng-template>

<!-- ng-content — content projection slot (like @RenderFragment in Blazor) -->
<!-- card.component.html: -->
<div class="card">
  <ng-content select="[header]"></ng-content>
  <ng-content></ng-content>
</div>

<!-- parent: -->
<app-card>
  <h2 header>Title</h2>
  <p>Body content</p>
</app-card>
```

---

### Q38. What are class and style bindings?

```html
<!-- Single class -->
<div [class.active]="isActive" [class.error]="hasError"></div>

<!-- Multiple classes — ngClass -->
<div [ngClass]="{ active: isActive, error: hasError, 'text-bold': isBold }"></div>
<div [ngClass]="['active', 'highlight']"></div>

<!-- Single style -->
<div [style.color]="textColor" [style.font-size.px]="fontSize"></div>

<!-- Multiple styles — ngStyle -->
<div [ngStyle]="{ color: textColor, 'font-size': fontSize + 'px' }"></div>
```

---

## 🏗️ SECTION 4: Directives

---

### Q39. What are the three types of Angular directives?

- **Component** — directive with a template (`@Component`). Every component is technically a directive.
- **Structural** — change DOM structure: `*ngIf`, `*ngFor`, `*ngSwitch`. The `*` is syntactic sugar for `<ng-template>`.
- **Attribute** — change appearance/behavior: `ngClass`, `ngStyle`, or custom `@Directive`.

---

### Q40. How does `*ngIf` work including the `else` branch?

```html
<!-- Basic -->
<div *ngIf="isLoggedIn">Welcome back!</div>

<!-- With else -->
<div *ngIf="user; else loading">Hello {{ user.name }}</div>
<ng-template #loading><p>Loading...</p></ng-template>

<!-- With local alias -->
<div *ngIf="user$ | async as user">Hello {{ user.name }}</div>
```

`*ngIf` **removes** the element from the DOM when false (not just hides it with CSS).

---

### Q41. How does `*ngFor` work and what is `trackBy`?

```html
<li *ngFor="let item of items; let i = index; let last = last; trackBy: trackById">
  {{ i + 1 }}. {{ item.name }}
  <span *ngIf="last"> (last)</span>
</li>
```

```typescript
// Without trackBy: Angular destroys and recreates all DOM nodes when list changes
// With trackBy: Angular reuses existing DOM nodes — massive performance win
trackById(index: number, item: Product): number {
  return item.id;
}
```

---

### Q42. How does `*ngSwitch` work?

```html
<div [ngSwitch]="status">
  <p *ngSwitchCase="'loading'">Loading...</p>
  <p *ngSwitchCase="'success'">Done!</p>
  <p *ngSwitchCase="'error'">Something went wrong</p>
  <p *ngSwitchDefault>Unknown state</p>
</div>
```

---

### Q43. How do you create a custom attribute directive?

```typescript
@Directive({ selector: '[appHighlight]', standalone: true })
export class HighlightDirective {
  @Input() appHighlight = 'yellow';

  constructor(private el: ElementRef, private renderer: Renderer2) {}

  @HostListener('mouseenter') onEnter() {
    this.renderer.setStyle(this.el.nativeElement, 'backgroundColor', this.appHighlight);
  }

  @HostListener('mouseleave') onLeave() {
    this.renderer.removeStyle(this.el.nativeElement, 'backgroundColor');
  }
}
```

```html
<p appHighlight="lightblue">Hover me</p>
```

> 💡 Use `Renderer2` instead of direct DOM manipulation — works with SSR and Web Workers.

---

### Q44. What are `@HostListener` and `@HostBinding`?

```typescript
@Directive({ selector: '[appActive]' })
export class ActiveDirective {
  @HostBinding('class.is-active') isActive = false;
  @HostBinding('attr.aria-pressed') get ariaPressed() { return this.isActive; }

  @HostListener('click') onClick() {
    this.isActive = !this.isActive;
  }

  @HostListener('document:keydown.escape') onEscape() {
    this.isActive = false;
  }
}
```

---

## 🔧 SECTION 5: Pipes

---

### Q45. What are pipes and how do you use them?

Pipes transform values for display in templates using `|`.

```html
<p>{{ price | currency:'INR':'symbol':'1.0-0' }}</p>
<p>{{ today | date:'dd MMM yyyy' }}</p>
<p>{{ 'hello world' | titlecase }}</p>
<p>{{ longText | slice:0:100 }}</p>
<p>{{ user | json }}</p>                     <!-- debugging -->
<p>{{ products$ | async }}</p>              <!-- auto-subscribe Observable -->
```

---

### Q46. What are all the built-in Angular pipes?

| Pipe | Example | Output |
|---|---|---|
| `date` | `date:'shortDate'` | 6/29/26 |
| `currency` | `currency:'USD'` | $99.99 |
| `number` | `number:'1.2-2'` | 1,234.56 |
| `percent` | `percent:'1.1-1'` | 95.5% |
| `uppercase` | `'hello' \| uppercase` | HELLO |
| `lowercase` | `'HELLO' \| lowercase` | hello |
| `titlecase` | `'hello world' \| titlecase` | Hello World |
| `slice` | `arr \| slice:0:3` | first 3 items |
| `json` | `obj \| json` | pretty JSON string |
| `async` | `obs$ \| async` | unwrapped value |
| `keyvalue` | `obj \| keyvalue` | `{key, value}` pairs |

---

### Q47. How do you create a custom pipe?

```typescript
@Pipe({ name: 'truncate', standalone: true, pure: true })
export class TruncatePipe implements PipeTransform {
  transform(value: string, limit: number = 50, suffix: string = '...'): string {
    if (!value) return '';
    return value.length > limit
      ? value.substring(0, limit) + suffix
      : value;
  }
}
```

```html
<p>{{ article.body | truncate:100:'…' }}</p>
```

---

### Q48. What is the difference between pure and impure pipes?

| | Pure (default) | Impure (`pure: false`) |
|---|---|---|
| Runs when | Input **reference** changes | Every CD cycle |
| Performance | Fast | Potentially slow |
| Use for | Most transforms | `async` pipe, filtering mutable arrays |

> ⚠️ Avoid impure pipes for sorting/filtering large arrays — do that in the component instead.

---

## 📡 SECTION 6: Component Communication

---

### Q49. How does `@Input()` work (parent → child)?

```typescript
// child.component.ts
@Input() title: string = '';
@Input({ required: true }) product!: Product;
@Input() set config(value: Config) {
  this._config = { ...defaultConfig, ...value };
}
```

```html
<!-- parent template -->
<app-child [title]="pageTitle" [product]="selectedProduct"></app-child>
```

---

### Q50. How does `@Output()` and `EventEmitter` work (child → parent)?

```typescript
// child.component.ts
@Output() productSelected = new EventEmitter<Product>();
@Output() deleted = new EventEmitter<number>();

selectProduct(p: Product): void {
  this.productSelected.emit(p);
}
```

```html
<!-- parent template -->
<app-child
  (productSelected)="onProductSelected($event)"
  (deleted)="onDelete($event)">
</app-child>
```

---

### Q51. What is `@ViewChild` and when do you use it?

```typescript
@ViewChild('nameInput') nameInput!: ElementRef<HTMLInputElement>;
@ViewChild(ChildComponent) child!: ChildComponent;
@ViewChild(NgForm) form!: NgForm;

// ✅ Access in ngAfterViewInit (not ngOnInit — view not ready yet)
ngAfterViewInit(): void {
  this.nameInput.nativeElement.focus();
  this.child.refresh();
}
```

---

### Q52. What is the difference between `@ViewChild` and `@ContentChild`?

```typescript
// @ViewChild — element in THIS component's own template
@ViewChild('myBtn') btn!: ElementRef;

// @ContentChild — content projected INTO this component via <ng-content>
@ContentChild(TabTitleComponent) tabTitle!: TabTitleComponent;

// Access ContentChild in ngAfterContentInit (not ngAfterViewInit)
ngAfterContentInit(): void {
  console.log(this.tabTitle.label);
}
```

---

### Q53. How do you communicate between sibling components (no parent)?

Use a **shared service with a `BehaviorSubject`**:

```typescript
@Injectable({ providedIn: 'root' })
export class CartService {
  private cartItems = new BehaviorSubject<CartItem[]>([]);
  cartItems$ = this.cartItems.asObservable();

  addItem(item: CartItem): void {
    this.cartItems.next([...this.cartItems.value, item]);
  }

  removeItem(id: number): void {
    this.cartItems.next(this.cartItems.value.filter(i => i.id !== id));
  }
}
```

Component A calls `addItem()`. Component B subscribes to `cartItems$` — automatically gets updates.

---

## 📋 SECTION 7: Forms

---

### Q54. What is the difference between Template-Driven and Reactive forms?

| Feature | Template-Driven | Reactive |
|---|---|---|
| Logic in | HTML template | TypeScript class |
| Module | `FormsModule` | `ReactiveFormsModule` |
| Testing | Harder | Easy (pure class) |
| Dynamic fields | Cumbersome | Easy with `FormArray` |
| Validation | HTML attributes | `Validators` in code |
| Best for | Simple forms | Complex / dynamic / enterprise |

---

### Q55. What are `FormGroup`, `FormControl`, and `FormArray`?

```typescript
// FormControl — single field
const nameCtrl = new FormControl('', Validators.required);

// FormGroup — object of named controls
const loginForm = new FormGroup({
  email: new FormControl('', [Validators.required, Validators.email]),
  password: new FormControl('', [Validators.required, Validators.minLength(8)])
});

// FormArray — dynamic list of controls
const phonesArray = new FormArray([
  new FormControl(''),
  new FormControl('')
]);
```

---

### Q56. What is `FormBuilder` and why use it?

Shorthand service for creating reactive forms — less verbose.

```typescript
constructor(private fb: FormBuilder) {}

ngOnInit(): void {
  this.form = this.fb.group({
    name:  ['', [Validators.required, Validators.minLength(2)]],
    email: ['', [Validators.required, Validators.email]],
    age:   [null, [Validators.min(0), Validators.max(120)]],
    address: this.fb.group({
      city:    [''],
      country: ['']
    }),
    phones: this.fb.array([this.fb.control('')])
  });
}
```

---

### Q57. What are the built-in validators?

```typescript
Validators.required
Validators.requiredTrue          // checkbox must be checked
Validators.email
Validators.minLength(8)
Validators.maxLength(50)
Validators.min(0)
Validators.max(100)
Validators.pattern('^[a-zA-Z]+$')
Validators.nullValidator         // always valid (placeholder)
```

---

### Q58. How do you create a custom validator?

```typescript
// Field-level validator
function noSpaces(control: AbstractControl): ValidationErrors | null {
  return /\s/.test(control.value) ? { noSpaces: true } : null;
}

// Cross-field validator (apply to FormGroup)
function passwordMatch(group: AbstractControl): ValidationErrors | null {
  const pass = group.get('password')?.value;
  const confirm = group.get('confirmPassword')?.value;
  return pass === confirm ? null : { passwordMismatch: true };
}

this.form = this.fb.group({
  username: ['', [Validators.required, noSpaces]],
  password: [''],
  confirmPassword: ['']
}, { validators: passwordMatch });
```

---

### Q59. What are the FormControl status flags and when do you use them?

```typescript
control.valid       // passes all validators
control.invalid     // fails at least one
control.touched     // user has blurred the field
control.untouched   // user hasn't blurred yet
control.dirty       // value has changed
control.pristine    // value hasn't changed
control.pending     // async validator running
control.errors      // object of error keys or null
```

```html
<!-- Show error only after user has interacted -->
<div *ngIf="form.get('email')?.invalid && form.get('email')?.touched">
  <span *ngIf="form.get('email')?.hasError('required')">Email is required</span>
  <span *ngIf="form.get('email')?.hasError('email')">Invalid email format</span>
</div>
```

---

### Q60. How do you submit a reactive form?

```html
<form [formGroup]="form" (ngSubmit)="onSubmit()">
  <input formControlName="email" />
  <input formControlName="password" type="password" />
  <button type="submit" [disabled]="form.invalid || isSaving">
    {{ isSaving ? 'Saving...' : 'Submit' }}
  </button>
</form>
```

```typescript
onSubmit(): void {
  if (this.form.invalid) return;
  this.isSaving = true;
  this.api.save(this.form.value).subscribe({
    next: () => { this.form.reset(); this.isSaving = false; },
    error: () => { this.isSaving = false; }
  });
}
```

---

## 🗺️ SECTION 8: Routing

---

### Q61. How do you set up routing in Angular?

```typescript
const routes: Routes = [
  { path: '',              component: HomeComponent },
  { path: 'products',      component: ProductListComponent },
  { path: 'products/:id',  component: ProductDetailComponent },
  { path: 'admin',         loadChildren: () => import('./admin/admin.module').then(m => m.AdminModule) },
  { path: '**',            component: NotFoundComponent }   // wildcard — MUST be last
];
```

---

### Q62. What are `router-outlet` and `routerLink`?

```html
<nav>
  <a routerLink="/" routerLinkActive="active" [routerLinkActiveOptions]="{ exact: true }">Home</a>
  <a [routerLink]="['/products', product.id]">Details</a>
  <a [routerLink]="['/products']" [queryParams]="{ page: 2 }">Page 2</a>
</nav>

<!-- Where matched component renders -->
<router-outlet></router-outlet>
```

---

### Q63. How do you read route and query parameters?

```typescript
constructor(private route: ActivatedRoute) {}

ngOnInit(): void {
  // Route param — snapshot (doesn't update if navigating to same route)
  const id = this.route.snapshot.params['id'];

  // Route param — Observable (updates on every navigation)
  this.route.paramMap.subscribe(p => this.loadProduct(+p.get('id')!));

  // Query params — ?page=2&size=10
  this.route.queryParamMap.subscribe(q => {
    this.page = +q.get('page')! || 1;
    this.size = +q.get('size')! || 10;
  });
}
```

---

### Q64. How do you navigate programmatically?

```typescript
constructor(private router: Router) {}

// Basic navigation
this.router.navigate(['/products']);
this.router.navigate(['/products', id]);

// With query params
this.router.navigate(['/products'], { queryParams: { page: 2 } });

// Relative navigation
this.router.navigate(['../detail', id], { relativeTo: this.route });

// Replace current history entry
this.router.navigate(['/home'], { replaceUrl: true });
```

---

### Q65. How do you set up nested (child) routes?

```typescript
const routes: Routes = [
  {
    path: 'admin',
    component: AdminLayoutComponent,
    children: [
      { path: '',         redirectTo: 'dashboard', pathMatch: 'full' },
      { path: 'dashboard', component: DashboardComponent },
      { path: 'users',    component: UsersComponent },
      { path: 'settings', component: SettingsComponent }
    ]
  }
];
```

Parent `AdminLayoutComponent` must have its own `<router-outlet>` for children to render.

---

### Q66. What is lazy loading and why use it?

Loads a feature module only when user navigates to it — reduces initial bundle size.

```typescript
// Lazy loading a module
{
  path: 'admin',
  loadChildren: () => import('./admin/admin.module').then(m => m.AdminModule)
}

// Lazy loading a standalone component (Angular 14+)
{
  path: 'product/:id',
  loadComponent: () => import('./product/product.component').then(m => m.ProductComponent)
}
```

---

### Q67. What are route guards?

Guards control whether navigation is allowed.

```typescript
// Functional guard (Angular 15+, preferred)
export const authGuard: CanActivateFn = (route, state) => {
  const auth = inject(AuthService);
  const router = inject(Router);

  if (auth.isLoggedIn()) return true;
  return router.createUrlTree(['/login'], { queryParams: { returnUrl: state.url } });
};

// Apply to route
{ path: 'admin', component: AdminComponent, canActivate: [authGuard] }
```

| Guard | When to Use |
|---|---|
| `CanActivate` | Auth check — can user enter route? |
| `CanActivateChild` | Same for child routes |
| `CanDeactivate` | Unsaved changes warning — can user leave? |
| `CanMatch` | Should lazy module even load? |
| `Resolve` | Pre-fetch data before activation |

---

### Q68. What is a Resolver?

Pre-fetches data before the route component activates — component has data ready in `ngOnInit`.

```typescript
@Injectable({ providedIn: 'root' })
export class ProductResolver implements Resolve<Product> {
  constructor(private api: ProductService) {}

  resolve(route: ActivatedRouteSnapshot): Observable<Product> {
    return this.api.getById(+route.params['id']);
  }
}

// Route config
{ path: 'products/:id', component: DetailComponent, resolve: { product: ProductResolver } }

// Component
ngOnInit(): void {
  this.product = this.route.snapshot.data['product'];
  // Or: this.route.data.subscribe(d => this.product = d['product']);
}
```

---

## 💉 SECTION 9: Services & Dependency Injection

---

### Q69. What is a service in Angular?

A class holding reusable business logic, data access, or shared state — separated from components.

```typescript
@Injectable({ providedIn: 'root' })
export class ProductService {
  private apiUrl = `${environment.apiUrl}/products`;

  constructor(private http: HttpClient) {}

  getAll(): Observable<Product[]> {
    return this.http.get<Product[]>(this.apiUrl);
  }

  getById(id: number): Observable<Product> {
    return this.http.get<Product>(`${this.apiUrl}/${id}`);
  }

  create(product: Omit<Product, 'id'>): Observable<Product> {
    return this.http.post<Product>(this.apiUrl, product);
  }
}
```

---

### Q70. What is Dependency Injection in Angular?

Angular's DI system provides instances of services automatically. When Angular sees a typed parameter in a constructor, it finds/creates that instance.

```typescript
// Angular creates UserService and injects it automatically
constructor(
  private userService: UserService,
  private router: Router,
  private fb: FormBuilder
) {}
```

> 💡 **For .NET devs:** Same concept as .NET's built-in DI container (`IServiceCollection`). `providedIn: 'root'` = `AddSingleton`.

---

### Q71. What are the different ways to provide services?

```typescript
// 1. Root-level singleton (preferred — tree-shakable)
@Injectable({ providedIn: 'root' })
export class AuthService { }

// 2. Module-level (shared within feature module)
@Injectable({ providedIn: UserModule })
export class UserHelperService { }

// 3. Component-level (new instance per component)
@Component({
  providers: [FormStateService]   // each instance gets its own
})
export class OrderFormComponent { }

// 4. In NgModule providers array
@NgModule({ providers: [LoggingService] })
```

---

### Q72. What is the hierarchical injector?

Angular has a tree of injectors mirroring the component tree. When a service is requested, Angular walks **up** the tree looking for a provider. Component-level providers override parent providers for that subtree.

---

## 🌐 SECTION 10: HTTP Client

---

### Q73. How do you set up and use `HttpClient`?

```typescript
// main.ts (standalone)
bootstrapApplication(AppComponent, {
  providers: [provideHttpClient(withInterceptorsFromDi())]
});

// Service usage
constructor(private http: HttpClient) {}

// All methods return Observables
this.http.get<Product[]>('/api/products')
this.http.get<Product>(`/api/products/${id}`)
this.http.post<Product>('/api/products', body)
this.http.put<Product>(`/api/products/${id}`, body)
this.http.patch<Product>(`/api/products/${id}`, partial)
this.http.delete<void>(`/api/products/${id}`)
```

---

### Q74. How do you send headers and query params?

```typescript
const headers = new HttpHeaders({
  'Authorization': `Bearer ${this.authService.getToken()}`,
  'Content-Type': 'application/json'
});

const params = new HttpParams()
  .set('page', page.toString())
  .set('size', '20')
  .set('sort', 'name,asc');

return this.http.get<PagedResult<Product>>('/api/products', { headers, params });
```

---

### Q75. What are HTTP Interceptors?

Middleware that runs on every request/response. Common uses: attach auth token, global error handling, loading spinner, logging.

```typescript
@Injectable()
export class AuthInterceptor implements HttpInterceptor {
  constructor(private auth: AuthService, private router: Router) {}

  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const token = this.auth.getToken();
    const authReq = token
      ? req.clone({ setHeaders: { Authorization: `Bearer ${token}` } })
      : req;

    return next.handle(authReq).pipe(
      catchError((error: HttpErrorResponse) => {
        if (error.status === 401) this.router.navigate(['/login']);
        return throwError(() => error);
      })
    );
  }
}

// Register in AppModule or providers
{ provide: HTTP_INTERCEPTORS, useClass: AuthInterceptor, multi: true }
```

---

### Q76. How do you handle HTTP errors?

```typescript
// In service — provide fallback
getProducts(): Observable<Product[]> {
  return this.http.get<Product[]>('/api/products').pipe(
    retry(2),
    catchError((err: HttpErrorResponse) => {
      console.error('API error:', err.status, err.message);
      return of([]);   // return empty array as fallback
    })
  );
}

// In component — handle in subscribe
this.productService.getAll().subscribe({
  next: (products) => this.products = products,
  error: (err) => this.errorMessage = err.message,
  complete: () => this.isLoading = false
});
```

---

## 🔁 SECTION 11: RxJS & Observables

---

### Q77. What is the difference between a Promise and an Observable?

| Feature | Promise | Observable |
|---|---|---|
| Values | Single value | Stream of 0 to many values |
| Execution | Eager (starts immediately) | Lazy (starts on subscribe) |
| Cancellable | ❌ | ✅ unsubscribe() |
| Operators | `.then`, `.catch` | Full RxJS pipeline |
| Multi-cast | Always | By default unicast |

---

### Q78. How do you subscribe and prevent memory leaks?

```typescript
// Option 1 — async pipe (BEST — auto-unsubscribes)
products$ = this.productService.getAll();
// template: *ngFor="let p of products$ | async"

// Option 2 — takeUntilDestroyed (Angular 16+, PREFERRED for manual subscriptions)
private destroyRef = inject(DestroyRef);

ngOnInit(): void {
  this.service.getData()
    .pipe(takeUntilDestroyed(this.destroyRef))
    .subscribe(data => this.data = data);
}

// Option 3 — takeUntil with Subject
private destroy$ = new Subject<void>();

ngOnInit(): void {
  this.service.getData()
    .pipe(takeUntil(this.destroy$))
    .subscribe(data => this.data = data);
}

ngOnDestroy(): void {
  this.destroy$.next();
  this.destroy$.complete();
}
```

---

### Q79. What are the most useful RxJS operators?

```typescript
// Transformation
map(user => user.name)                 // transform each value
pluck('name')                          // shorthand: pick a property
scan((acc, val) => [...acc, val], [])  // like reduce but emits each step

// Filtering
filter(user => user.isActive)          // keep matching values
distinctUntilChanged()                 // skip if value didn't change
debounceTime(300)                      // wait for pause (search input)
throttleTime(1000)                     // emit max once per time window
take(5)                                // take first 5 then complete

// Side effects
tap(val => console.log(val))           // side effects without changing stream

// Error handling
catchError(err => of([]))              // handle error, return fallback
retry(3)                               // retry on error

// Combination
forkJoin([obs1$, obs2$])              // wait for ALL to complete
combineLatest([obs1$, obs2$])         // emit when ANY emits
merge(obs1$, obs2$)                   // emit from all simultaneously
```

---

### Q80. What is the difference between `switchMap`, `mergeMap`, `concatMap`, and `exhaustMap`?

All map each emission to a new Observable.

| Operator | Behavior | Use Case |
|---|---|---|
| `switchMap` | Cancels previous, switches to new | Search-as-you-type |
| `mergeMap` | All run in parallel | Independent parallel requests |
| `concatMap` | Queue and run in order | Sequential file uploads |
| `exhaustMap` | Ignores new while one is in flight | Login button (prevent double-submit) |

```typescript
// Search example — switchMap cancels the previous HTTP request
this.searchControl.valueChanges.pipe(
  debounceTime(300),
  distinctUntilChanged(),
  switchMap(term => this.api.search(term))
).subscribe(results => this.results = results);
```

---

### Q81. What is the difference between `Subject`, `BehaviorSubject`, `ReplaySubject`, `AsyncSubject`?

All are both Observable and Observer (you can push and subscribe).

```typescript
// Subject — no initial value, no memory
const s = new Subject<number>();
s.subscribe(v => console.log(v));
s.next(1);   // subscriber gets 1

// BehaviorSubject — holds current value, new subscribers get it immediately
const bs = new BehaviorSubject<number>(0);   // 0 is initial value
bs.subscribe(v => console.log(v));           // immediately gets 0
bs.next(5);                                  // gets 5
console.log(bs.value);                       // read current synchronously

// ReplaySubject(n) — replays last n values to new subscribers
const rs = new ReplaySubject<number>(3);

// AsyncSubject — emits only last value, only on complete
const as = new AsyncSubject<number>();
```

> 💡 `BehaviorSubject` is the most useful for **shared state** in services.

---

### Q82. How do you implement search-as-you-type with RxJS?

```typescript
// Component
searchControl = new FormControl('');

results$ = this.searchControl.valueChanges.pipe(
  debounceTime(300),
  distinctUntilChanged(),
  filter(term => (term ?? '').length >= 2),
  switchMap(term =>
    this.productService.search(term!).pipe(
      catchError(() => of([]))   // don't break stream on error
    )
  )
);
```

```html
<input [formControl]="searchControl" placeholder="Search..." />
<ul>
  <li *ngFor="let r of results$ | async">{{ r.name }}</li>
</ul>
```

---

### Q83. What is the `async` pipe and why prefer it over manual subscribe?

```typescript
// In component — no subscribe, no unsubscribe needed
products$ = this.productService.getAll();
user$ = this.userService.getCurrentUser();
```

```html
<!-- Auto-subscribes AND auto-unsubscribes on destroy -->
<div *ngIf="user$ | async as user">Hello {{ user.name }}</div>
<li *ngFor="let p of products$ | async">{{ p.name }}</li>
```

**Benefits:** No `ngOnDestroy` cleanup, no memory leaks, triggers OnPush CD automatically.

---

## ⚡ SECTION 12: Performance & Optimization

---

### Q84. How does Angular change detection work?

Angular checks every component on every async event (click, HTTP response, timer) to see what changed and update the DOM. **Zone.js** patches async APIs (setTimeout, Promises, XHR) and notifies Angular to run change detection.

Default: entire component tree is checked top-down on every event.

---

### Q85. What is `ChangeDetectionStrategy.OnPush`?

Angular **skips** the component during CD unless:
- An `@Input` received a **new reference** (not a mutated object)
- An event fired inside the component
- An Observable with `async` pipe emits
- `markForCheck()` or `detectChanges()` is called manually

```typescript
@Component({
  changeDetection: ChangeDetectionStrategy.OnPush,
  // ...
})
export class ProductCardComponent {
  @Input() product!: Product;    // must pass new reference to trigger update
}
```

> 💡 Use OnPush on all leaf/display components (cards, list items). It dramatically reduces CD work.

---

### Q86. What is the difference between AOT and JIT compilation?

| | JIT (Just-In-Time) | AOT (Ahead-Of-Time) |
|---|---|---|
| When | Browser at runtime | Build time |
| Bundle | Larger (includes compiler) | Smaller |
| Startup | Slower | Faster |
| Errors | Caught at runtime | Caught at build time |
| Default | Dev mode | Production build |

---

### Q87. What are some Angular performance best practices?

- Use `OnPush` change detection on display components
- Use `trackBy` on `*ngFor` for large lists
- Lazy load feature modules/components
- Use `async` pipe instead of manual subscribe
- Use `takeUntilDestroyed` to prevent memory leaks
- Avoid heavy computation in templates
- Use virtual scrolling (`@angular/cdk/scrolling`) for large lists
- Use `ng-container` instead of wrapper divs
- Memoize expensive pipe transforms (keep pipes pure)

---

### Q88. What is Tree Shaking?

Build step that removes unused code from the final bundle. Angular CLI (esbuild/Webpack) analyzes imports and drops everything not used.

```typescript
// ✅ Named imports — tree-shakable
import { map, filter } from 'rxjs/operators';

// ❌ Avoid barrel imports of large libraries
import * as _ from 'lodash';   // imports everything
```

---

## 🔐 SECTION 13: Security

---

### Q89. How does Angular protect against XSS?

Angular automatically sanitizes all values bound via `{{ }}` interpolation and property bindings — dangerous HTML is escaped.

```typescript
// ✅ Safe — Angular escapes HTML
this.content = '<script>alert("xss")</script>';
// template: {{ content }} → renders as text, not script

// ⚠️ Bypass only for trusted content YOU control (never user input)
constructor(private sanitizer: DomSanitizer) {}

getSafeHtml(html: string): SafeHtml {
  return this.sanitizer.bypassSecurityTrustHtml(html);
}
```

---

### Q90. How does Angular handle CSRF?

Angular's `HttpClient` reads the `XSRF-TOKEN` cookie and sends it as `X-XSRF-TOKEN` header on non-GET requests automatically. The server validates it.

With stateless JWT auth (tokens in headers, not cookies), CSRF is not a concern.

---

## 🏗️ SECTION 14: Signals (Angular 16+)

---

### Q91. What are Angular Signals?

A reactive primitive for state management — a simpler alternative to Observables for component state.

```typescript
import { signal, computed, effect } from '@angular/core';

// Signal — reactive state variable
count = signal(0);
name = signal<string>('Purushottam');

// Read
console.log(this.count());         // call as function

// Write
this.count.set(5);
this.count.update(c => c + 1);

// Computed — derived signal (lazy, memoized)
doubled = computed(() => this.count() * 2);

// Effect — side effect when signal changes
effect(() => {
  console.log('Count changed to:', this.count());
});
```

---

### Q92. What is the difference between Signals and Observables?

| | Signals | Observables |
|---|---|---|
| Concept | Reactive state value | Stream of events |
| Read | `signal()` — synchronous | `.subscribe()` — async |
| Change detection | Automatic, fine-grained | Needs `async` pipe or manual |
| Best for | Component state, UI state | Async events, HTTP, streams |
| Interop | `toObservable(signal)` / `toSignal(obs$)` | ← |

---

## 📦 SECTION 15: State Management & Architecture

---

### Q93. When should you use a shared service vs NgRx?

**Shared Service with BehaviorSubject** — good for:
- Small/medium apps
- State scoped to a feature
- Simple cart, auth state, theme

**NgRx** — good for:
- Large, complex apps with lots of shared state
- Redux DevTools time-travel debugging
- Team needing strict conventions

---

### Q94. What is NgRx and what are its core pieces?

Redux-style state management for Angular.

```
Action → Reducer → Store → Selector → Component
           ↑                              ↓
         Effect ←————————————————— (async side effects)
```

```typescript
// Action
export const loadProducts = createAction('[Products] Load');
export const loadProductsSuccess = createAction('[Products] Load Success',
  props<{ products: Product[] }>());

// Reducer
const reducer = createReducer(initialState,
  on(loadProductsSuccess, (state, { products }) => ({ ...state, products }))
);

// Selector
export const selectProducts = createSelector(
  selectProductsState, state => state.products
);

// Effect
loadProducts$ = createEffect(() =>
  this.actions$.pipe(
    ofType(loadProducts),
    switchMap(() => this.api.getAll().pipe(
      map(products => loadProductsSuccess({ products }))
    ))
  )
);
```

---

### Q95. What is a Smart vs Dumb (Presentational) component pattern?

```
Smart Component (Container)        Dumb Component (Presentational)
─────────────────────────          ──────────────────────────────
Knows about services               No service injection
Fetches data                       Receives data via @Input only
Handles business logic             Emits events via @Output only
Passes data to children            Pure UI — easily testable
Uses OnPush with Observables       Always use OnPush
```

This separation makes components reusable and testable.

---

## 🔧 SECTION 16: Build, Environments & Upgrade

---

### Q96. How do you use environment files?

```typescript
// src/environments/environment.ts (dev)
export const environment = {
  production: false,
  apiUrl: 'http://localhost:5001/api'
};

// src/environments/environment.prod.ts
export const environment = {
  production: true,
  apiUrl: 'https://api.myapp.com'
};

// Usage in service
import { environment } from '../environments/environment';

private apiUrl = environment.apiUrl;  // Angular CLI swaps file at build time
```

```bash
ng build                              # uses environment.ts
ng build --configuration production   # uses environment.prod.ts
```

---

### Q97. What does `ng build --configuration production` do?

- **AOT compilation** — templates compiled at build time
- **Tree shaking** — unused code removed
- **Minification + uglification** — smaller bundle
- **File replacement** — environment.prod.ts swapped in
- **Source maps** — optional for debugging prod
- Output goes to `dist/` — deploy to any static host (Nginx, Azure Static Web Apps, S3, IIS)

---

## 🧪 SECTION 17: Testing Basics

---

### Q98. What is the difference between unit tests and integration tests in Angular?

```typescript
// Unit test — test class in isolation with mocks
it('should add item to cart', () => {
  const service = new CartService();
  service.addItem({ id: 1, name: 'Test', price: 10 });
  expect(service.cartItems.length).toBe(1);
});

// Integration test — test with Angular TestBed
beforeEach(async () => {
  await TestBed.configureTestingModule({
    declarations: [ProductCardComponent],
    imports: [RouterTestingModule]
  }).compileComponents();
});

it('should display product name', () => {
  const fixture = TestBed.createComponent(ProductCardComponent);
  fixture.componentInstance.product = mockProduct;
  fixture.detectChanges();
  const el = fixture.nativeElement.querySelector('h2');
  expect(el.textContent).toContain('Test Product');
});
```

---

### Q99. How do you mock a service in Angular tests?

```typescript
// Create spy object — all methods are automatically spied
const productServiceSpy = jasmine.createSpyObj('ProductService', ['getAll', 'getById']);
productServiceSpy.getAll.and.returnValue(of([mockProduct]));

await TestBed.configureTestingModule({
  declarations: [ProductListComponent],
  providers: [
    { provide: ProductService, useValue: productServiceSpy }
  ]
}).compileComponents();
```

---

### Q100. What is the difference between `detectChanges()` and `fixture.autoDetectChanges()`?

```typescript
// Manual change detection — you control when DOM updates
fixture.detectChanges();

// After input change — trigger manually
fixture.componentInstance.product = newProduct;
fixture.detectChanges();   // DOM updates now
const title = fixture.nativeElement.querySelector('h1').textContent;

// Auto detect — Angular triggers CD automatically (closer to real app)
fixture.autoDetectChanges(true);
```

---

## 📋 QUICK REFERENCE CARD

### TypeScript Cheatsheet

| Concept | Syntax | Angular Usage |
|---|---|---|
| Interface | `interface User {}` | API models, form shapes |
| Type alias | `type Status = 'a' \| 'b'` | Unions, enums |
| Enum | `enum Role { Admin = 'ADMIN' }` | Fixed constants |
| Generic | `<T>` | Services, HTTP, Signals |
| Utility types | `Partial<T>`, `Omit<T, 'k'>` | Forms, DTOs |
| Optional chain | `user?.name` | Templates, components |
| Non-null | `el!` | `@ViewChild`, `@Input` |
| Access mods | `private`, `public`, `readonly` | Class members |
| Async/await | `async fn(): Promise<T>` | Rarely; prefer Observables |

### Angular Cheatsheet

| Topic | Key Point |
|---|---|
| Constructor vs ngOnInit | Constructor = DI only; ngOnInit = logic |
| @Input / @Output | Parent→Child / Child→Parent |
| @ViewChild | Access after `ngAfterViewInit` |
| async pipe | Always prefer over manual subscribe |
| OnPush | Use on all display components |
| trackBy | Required on *ngFor with dynamic data |
| Lazy loading | `loadChildren` / `loadComponent` |
| Guards | Functional guards preferred (Angular 15+) |
| BehaviorSubject | Standard for shared state in services |
| switchMap | Search; mergeMap = parallel; exhaustMap = login btn |
| Signals | Use for component state (Angular 16+) |
| providedIn: 'root' | Singleton service (like AddSingleton in .NET) |