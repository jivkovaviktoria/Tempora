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

## Core Principles

**Tempora** is built around a few key principles:
- Deterministic behavior
Given the same inputs, Tempora always produces the same result.

- Business-aware scheduling
Supports business calendars, holidays, weekends, and working days.

- Time zone safety
Uses explicit time zone handling and avoids implicit system time.

- Pure calculation, no side effects
Tempora calculates when something should happen — it does not execute jobs.

- Testability first
All time-based logic is designed to be tested deterministically.

## What Tempora Is and Is Not
### What Tempora Is

- A scheduling and reminder calculation engine
- A reusable domain library
- Framework-agnostic
- Suitable for APIs, background jobs, and services

### What Tempora Is Not

- A job runner
- A background service
- A cron replacement
- A notification system

**Tempora** focuses strictly on reasoning about time, not executing actions.

## Example (Conceptual)
```
var rule = ReminderRule
    .Weekly(DayOfWeek.Monday)
    .At(10, 30)
    .InTimeZone("Europe/Sofia");

var calendar = BusinessCalendar
    .Default()
    .ExcludeWeekends()
    .WithHolidays(holidayProvider);

var nextExecution = ReminderEngine.CalculateNext(
    rule,
    lastExecution,
    now,
    calendar
);
```

## Key Features (Planned)
- Fluent API for defining reminder and scheduling rules
- Support for recurring schedules (daily, weekly, monthly)
- Business calendar awareness (weekends, holidays)
- Time zone–aware calculations
- Missed execution handling strategies
- Deterministic and test-friendly design

## Motivation

**Tempora** was created to address recurring problems found in real-world business systems:
- Incorrect next-run calculations
- Inconsistent reminder logic across services
- Difficult-to-test time-based behavior

By extracting this logic into a focused library, **Tempora** aims to make scheduling predictable, reusable, and correct.

## License
This project is licensed under the MIT License.
