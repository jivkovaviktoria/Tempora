# Tempora
A .NET library for business-aware reminder scheduling and time-based rule evaluation.

## Overview

Most business applications need to schedule reminders, deadlines, and periodic tasks:
- Weekly or monthly reports
- Time-based notifications
- SLA checks and follow-ups
- Actions triggered after specific events

Despite being common, time-based logic is often:
- Hard-coded and duplicated
- Incorrect around time zones and daylight saving changes
- Difficult to test
- Fragile when executions are missed

**Tempora** is a lightweight **.NET library** that focuses on **correctness, determinism, and clarity** when working with time-based rules and reminders.

It provides a clean, extensible way to define scheduling rules and calculate the next valid execution time under real business constraints.

## Features (v1.0.0)

- Weekly scheduling (single or multiple weekdays)
- Monthly scheduling on a fixed day of month (1–31)
- Time zone–aware execution calculation
- Business calendar support with weekend exclusion
- Default roll-forward behavior for non-business days
- Deterministic, test-driven calculation engine

---

## What Tempora Is (and Is Not)

### ✔ Tempora **is**
- A scheduling **calculation engine**
- Deterministic and side-effect free
- Explicit about business rules and defaults
- Easy to test and reason about

### ✖ Tempora **is not**
- A background job runner
- A cron replacement
- A persistence or storage solution
- A framework with hidden behavior

---

## Installation

Tempora is currently distributed as source.
NuGet packaging is planned for a future version.

---

## Usage

### Weekly scheduling

Schedule a reminder to run every Monday and Wednesday at 10:30 UTC:

```csharp
var rule = ReminderRule
    .Weekly(DayOfWeek.Monday, DayOfWeek.Wednesday)
    .At(10, 30)
    .InTimeZone("UTC");

var calendar = BusinessCalendar.Default();

var nextExecution = ReminderEngine.CalculateNext(
    rule,
    lastExecution: null,
    now: DateTimeOffset.UtcNow,
    calendar);
```

### Monthly scheduling

Schedule a reminder to run on the 15th of every month:

```csharp
var rule = ReminderRule
    .Monthly(15)
    .At(10, 30)
    .InTimeZone("UTC");

var calendar = BusinessCalendar.Default();

var nextExecution = ReminderEngine.CalculateNext(
    rule,
    lastExecution: null,
    now: DateTimeOffset.UtcNow,
    calendar);
```

### Business calendar (weekend exclusion)

Exclude weekends from execution:

```csharp
var calendar = BusinessCalendar
    .Default()
    .ExcludeWeekends();
```

If a scheduled execution falls on a Saturday or Sunday, it is **rolled forward
to the next business day** by default.

**This behavior is intentional and documented.**

By extracting this logic into a focused library, **Tempora** aims to make scheduling predictable, reusable, and correct.

## License
This project is licensed under the MIT License.
