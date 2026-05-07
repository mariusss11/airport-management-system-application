# 🎨 Airport Flight Management - BMW-M Inspired Design

## Color Palette (Premium Luxury Theme)

```
🎯 Primary Colors:
   - Deep Black (BG): #0A0E27 (ultra dark, luxurious background)
   - Dark Charcoal: #1a1f3a (secondary background)
   - M-Sport Red: #DC143C (accent, premium highlight)
   - Metallic Silver: #D4AF37 (gold accents, premium feel)
   - Tech Blue: #00B4D8 (secondary accent)

🎨 Text Colors:
   - White Primary: #FFFFFF (main text)
   - Light Gray: #E0E0E0 (secondary text)
   - Muted Gray: #8B8B8B (tertiary text)
```

---

## 📐 Layout & Typography

```
TYPOGRAPHY:
├─ Titles: Segoe UI / Roboto Bold, 28-32px, Letter-spacing: 1px
├─ Headings: Segoe UI / Roboto Bold, 18-20px
├─ Body Text: Segoe UI / Roboto Regular, 14px
└─ Labels: Segoe UI / Roboto Regular, 12px

SPACING:
├─ Margin: 24px, 16px, 12px, 8px
├─ Padding: 16px, 12px, 8px
└─ Border Radius: 8-12px (smooth, premium feel)
```

---

## 🎪 MAIN APPLICATION LAYOUT

```
┌─────────────────────────────────────────────────────────────────┐
│  AEROPORT MANAGEMENT          🔔  👤 Settings                   │ Header
├─────┬───────────────────────────────────────────────────────────┤
│     │                                                             │
│ NAV │              MAIN CONTENT AREA                             │
│ ─── │                                                             │
│ 📊  │              (Content Changes Based on Selection)           │
│ ✈️  │                                                             │
│ 🔍  │                                                             │
│ 📋  │                                                             │
│ 📁  │                                                             │
│ ⚙️  │                                                             │
│     │                                                             │
└─────┴───────────────────────────────────────────────────────────┘
```

---

## 🏠 SCREEN 1: DASHBOARD (Premium Overview)

```
┌────────────────────────────────────────────────────────────────────┐
│  DASHBOARD                                                          │
├────────────────────────────────────────────────────────────────────┤
│                                                                      │
│  Welcome to AEROPORT Management System                              │
│  Real-time Flight Operations                                        │
│                                                                      │
│  ┌──────────────────┐ ┌──────────────────┐ ┌──────────────────┐   │
│  │ 📊 ACTIVE FLIGHTS│ │ 🛫 DEPARTURES    │ │ ✈️ PLANES TODAY  │   │
│  │                  │ │                  │ │                  │   │
│  │      42          │ │      28          │ │       8          │   │
│  └──────────────────┘ └──────────────────┘ └──────────────────┘   │
│                                                                      │
│  ┌──────────────────┐ ┌──────────────────┐ ┌──────────────────┐   │
│  │ 💺 SEATS AVAILABLE│ │ 📈 AVG PRICE     │ │ ⏱️  LONGEST FLIGHT│   │
│  │                  │ │                  │ │                  │   │
│  │      1,245       │ │      €145.50     │ │   4h 35m         │   │
│  └──────────────────┘ └──────────────────┘ └──────────────────┘   │
│                                                                      │
│  ╔════════════════════════════════════════════════════════════╗   │
│  ║  TODAY'S FLIGHT SCHEDULE                                   ║   │
│  ╠═════════════════╦═════════════════╦═════════════════════════╣   │
│  ║ FLIGHT CODE     ║ DESTINATION     ║ DEPARTURE │ STATUS     ║   │
│  ╠═════════════════╬═════════════════╬═════════════════════════╣   │
│  ║ FR-101          ║ Paris           ║ 08:30     │ On Time    ║   │
│  ║ RO-205          ║ Madrid          ║ 10:15     ║ On Time    ║   │
│  ║ IT-312          ║ Rome            ║ 12:00     ║ Delayed 5m ║   │
│  ║ GR-408          ║ Athens          ║ 14:30     ║ On Time    ║   │
│  ╚═════════════════╩═════════════════╩═════════════════════════╝   │
│                                                                      │
└────────────────────────────────────────────────────────────────────┘
```

---

## ✈️ SCREEN 2: FLIGHT MANAGEMENT (Add/Edit Premium Form)

```
┌────────────────────────────────────────────────────────────────────┐
│  FLIGHT MANAGEMENT                     [+] NEW FLIGHT              │
├────────────────────────────────────────────────────────────────────┤
│                                                                      │
│  FLIGHT INFORMATION                                                 │
│  ─────────────────────────────────────────────────────────────────  │
│                                                                      │
│  Flight Code *                 Destination *                        │
│  ┌─────────────────────────┐  ┌─────────────────────────┐          │
│  │ FR-                     │  │ Paris                   │          │
│  └─────────────────────────┘  └─────────────────────────┘          │
│                                                                      │
│  Departure Time *              Arrival Time *                       │
│  ┌─────────────────────────┐  ┌─────────────────────────┐          │
│  │ 08:30 ⏰                │  │ 10:45 ⏰                │          │
│  └─────────────────────────┘  └─────────────────────────┘          │
│                                                                      │
│  Plane ID *                    Total Seats *                        │
│  ┌─────────────────────────┐  ┌─────────────────────────┐          │
│  │ AIR-001                 │  │ 180                     │          │
│  └─────────────────────────┘  └─────────────────────────┘          │
│                                                                      │
│  Ticket Price (€) *            Available Seats                      │
│  ┌─────────────────────────┐  ┌─────────────────────────┐          │
│  │ 145.50                  │  │ 24 (13%)  ▓▓░░░░░░░░  │          │
│  └─────────────────────────┘  └─────────────────────────┘          │
│                                                                      │
│  Day of Week *                 Status                               │
│  ┌─────────────────────────┐  ┌─────────────────────────┐          │
│  │ [▼] Monday              │  │ [▼] On Time             │          │
│  └─────────────────────────┘  └─────────────────────────┘          │
│                                                                      │
│  ┌─────────────┐ ┌─────────────┐ ┌─────────────┐ ┌──────────────┐ │
│  │ ✓ SAVE      │ │ ✕ CANCEL    │ │ 🗑️  DELETE   │ │ 📋 DUPLICATE │ │
│  └─────────────┘ └─────────────┘ └─────────────┘ └──────────────┘ │
│                                                                      │
└────────────────────────────────────────────────────────────────────┘
```

---

## 🔍 SCREEN 3: ADVANCED SEARCH & FILTER

```
┌────────────────────────────────────────────────────────────────────┐
│  FLIGHT SEARCH                                                      │
├────────────────────────────────────────────────────────────────────┤
│                                                                      │
│  SEARCH CRITERIA                                                    │
│  ─────────────────────────────────────────────────────────────────  │
│                                                                      │
│  🎯 Destination                Departure Time Range                │
│  ┌─────────────────────────┐  ┌──────────┐  to  ┌──────────┐      │
│  │ Paris, Madrid, Rome...  │  │ 08:00    │      │ 18:00    │      │
│  └─────────────────────────┘  └──────────┘      └──────────┘      │
│                                                                      │
│  📅 Days of Week               Minimum Seats Available              │
│  ┌─ ○ Mon  ○ Tue  ○ Wed ─┐   ┌─────────────────────────┐         │
│  │ ○ Thu  ○ Fri  ○ Sat  │   │ [slider] ──●──────── 50 │         │
│  │ ○ Sun  [All Week]     │   └─────────────────────────┘         │
│  └───────────────────────┘                                          │
│                                                                      │
│  💰 Price Range                Status Filter                        │
│  ┌──────────┐  to  ┌──────────┐  ☑ On Time   ☑ Delayed            │
│  │ €0       │      │ €500      │  ☑ Canceled ☑ All                 │
│  └──────────┘      └──────────┘                                     │
│                                                                      │
│  ┌──────────────────┐ ┌──────────────────┐ ┌──────────────────┐   │
│  │ 🔍 SEARCH        │ │ 🔄 RESET FILTERS │ │ 💾 SAVE SEARCH   │   │
│  └──────────────────┘ └──────────────────┘ └──────────────────┘   │
│                                                                      │
│  ╔════════════════════════════════════════════════════════════╗   │
│  ║  SEARCH RESULTS (18 flights found)                         ║   │
│  ╠══════════════════════════════════════════════════════════════╣  │
│  ║ FLIGHT │ DESTINATION │ DAY      │ DEPT  │ ARR   │ SEATS │   ║  │
│  ╠════════╬═════════════╬══════════╬═══════╬═══════╬═══════╣   ║  │
│  ║ FR-101 │ Paris       │ Monday   │ 08:30 │ 10:45 │ 24/180║   ║  │
│  ║ FR-102 │ Paris       │ Tuesday  │ 08:30 │ 10:45 │ 18/200║   ║  │
│  ║ SP-203 │ Madrid      │ Monday   │ 10:15 │ 13:00 │  5/150║   ║  │
│  ╚════════╩═════════════╩══════════╩═══════╩═══════╩═══════╝   ║  │
│                                                                      │
└────────────────────────────────────────────────────────────────────┘
```

---

## 📋 SCREEN 4: REPORTS & EXPORT

```
┌────────────────────────────────────────────────────────────────────┐
│  REPORTS & EXPORT                                                   │
├────────────────────────────────────────────────────────────────────┤
│                                                                      │
│  GENERATE REPORTS                                                   │
│  ─────────────────────────────────────────────────────────────────  │
│                                                                      │
│  ┌──────────────────────────────────────────────────────────────┐  │
│  │ 📅 MONDAY SCHEDULE                                           │  │
│  │ Export all flights scheduled for Monday                      │  │
│  │                                                              │  │
│  │                          ┌──────────────┐ ┌───────────────┐ │  │
│  │                          │📄 WORD (.doc)│ │📊 EXCEL (.csv)│ │  │
│  │                          └──────────────┘ └───────────────┘ │  │
│  └──────────────────────────────────────────────────────────────┘  │
│                                                                      │
│  ┌──────────────────────────────────────────────────────────────┐  │
│  │ 💺 AVAILABLE SEATS ANALYSIS                                 │  │
│  │ Current seat availability for all flights                    │  │
│  │                                                              │  │
│  │                          ┌──────────────┐ ┌───────────────┐ │  │
│  │                          │📄 WORD (.doc)│ │📊 EXCEL (.csv)│ │  │
│  │                          └──────────────┘ └───────────────┘ │  │
│  └──────────────────────────────────────────────────────────────┘  │
│                                                                      │
│  ┌──────────────────────────────────────────────────────────────┐  │
│  │ ⏱️  LONGEST FLIGHT DURATION                                  │  │
│  │ Find flights with maximum flight time                        │  │
│  │                                                              │  │
│  │                          ┌──────────────┐ ┌───────────────┐ │  │
│  │                          │📄 WORD (.doc)│ │📊 EXCEL (.csv)│ │  │
│  │                          └──────────────┘ └───────────────┘ │  │
│  └──────────────────────────────────────────────────────────────┘  │
│                                                                      │
│  ┌──────────────────────────────────────────────────────────────┐  │
│  │ 💰 AVERAGE TICKET PRICE BY DESTINATION                      │  │
│  │ Statistical analysis of pricing                              │  │
│  │                                                              │  │
│  │                          ┌──────────────┐ ┌───────────────┐ │  │
│  │                          │📄 WORD (.doc)│ │📊 EXCEL (.csv)│ │  │
│  │                          └──────────────┘ └───────────────┘ │  │
│  └──────────────────────────────────────────────────────────────┘  │
│                                                                      │
│  ┌──────────────────────────────────────────────────────────────┐  │
│  │ ✈️  PLANES CURRENTLY AT AIRPORT                             │  │
│  │ Real-time aircraft status and location                       │  │
│  │                                                              │  │
│  │                          ┌──────────────┐ ┌───────────────┐ │  │
│  │                          │📄 WORD (.doc)│ │📊 EXCEL (.csv)│ │  │
│  │                          └──────────────┘ └───────────────┘ │  │
│  └──────────────────────────────────────────────────────────────┘  │
│                                                                      │
└────────────────────────────────────────────────────────────────────┘
```

---

## 📁 SCREEN 5: ARCHIVED FLIGHTS

```
┌────────────────────────────────────────────────────────────────────┐
│  ARCHIVED FLIGHTS (Canceled/Deleted)                               │
├────────────────────────────────────────────────────────────────────┤
│                                                                      │
│  Filter by Date:  From [__/__/__] To [__/__/__]  [Search]         │
│                                                                      │
│  ╔════════════════════════════════════════════════════════════╗   │
│  ║ FLIGHT CODE │ DESTINATION │ CANCELED DATE │ REASON         ║   │
│  ╠═════════════╬═════════════╬═══════════════╬════════════════╣   │
│  ║ FR-099      │ Paris       │ 2024-05-15    │ Bad Weather    ║   │
│  ║ IT-312      │ Rome        │ 2024-05-14    │ Engine Issue   ║   │
│  ║ GR-405      │ Athens      │ 2024-05-12    │ Crew Absence   ║   │
│  ║ SP-201      │ Barcelona   │ 2024-05-10    │ Technical      ║   │
│  ╚════════════╩═════════════╩═══════════════╩════════════════╝   │
│                                                                      │
│  ┌─────────────┐ ┌──────────────────┐ ┌──────────────────┐       │
│  │ 🔄 RESTORE  │ │ 📄 VIEW DETAILS  │ │ 🗑️  DELETE PERM. │       │
│  └─────────────┘ └──────────────────┘ └──────────────────┘       │
│                                                                      │
└────────────────────────────────────────────────────────────────────┘
```

---

## ⚙️ SCREEN 6: SETTINGS & ADMINISTRATION

```
┌────────────────────────────────────────────────────────────────────┐
│  SETTINGS & ADMINISTRATION                                          │
├────────────────────────────────────────────────────────────────────┤
│                                                                      │
│  🔐 DATABASE SECURITY                                               │
│  ─────────────────────────────────────────────────────────────────  │
│  ☑ Enable Password Protection    [Change Password]                 │
│  ☑ Automatic Backup              [Configure]                       │
│  ☑ Data Encryption               [Enable SSL/TLS]                  │
│  ☑ User Logging & Audit Trail    [View Logs]                       │
│                                                                      │
│  💾 DATABASE MAINTENANCE                                            │
│  ─────────────────────────────────────────────────────────────────  │
│  Last Backup: 2024-05-15 22:30                                      │
│  ┌──────────────────┐ ┌──────────────────┐ ┌──────────────────┐   │
│  │ 💾 BACKUP NOW    │ │ 🔧 OPTIMIZE DB   │ │ 🔄 RESTORE DB    │   │
│  └──────────────────┘ └──────────────────┘ └──────────────────┘   │
│                                                                      │
│  👥 USER MANAGEMENT                                                 │
│  ─────────────────────────────────────────────────────────────────  │
│  ┌──────────────────────────────────────────────────────────────┐  │
│  │ USERNAME    │ ROLE          │ LAST LOGIN      │ STATUS       │  │
│  │─────────────┼───────────────┼─────────────────┼──────────────│  │
│  │ admin       │ Administrator │ 2024-05-15 08:30│ ✓ Active     │  │
│  │ operator1   │ Operator      │ 2024-05-15 07:15│ ✓ Active     │  │
│  │ analyst     │ Analyst       │ 2024-05-14 16:45│ ✓ Active     │  │
│  └──────────────────────────────────────────────────────────────┘  │
│                                                                      │
│  ┌──────────────────┐ ┌──────────────────┐                         │
│  │ ➕ ADD USER      │ │ 🔐 PERMISSIONS   │                         │
│  └──────────────────┘ └──────────────────┘                         │
│                                                                      │
│  🎨 APPEARANCE                                                      │
│  ─────────────────────────────────────────────────────────────────  │
│  Theme:  [◉ Dark Mode  ○ Light Mode  ○ Auto]                      │
│  Accent Color: [■ M-Sport Red  ○ Tech Blue  ○ Metallic Silver]   │
│  Font Size: [●────────────] 14px                                   │
│                                                                      │
│  ┌──────────────────┐ ┌──────────────────┐ ┌──────────────────┐   │
│  │ 💾 SAVE CHANGES  │ │ ⟲ RESET DEFAULTS │ │ ✕ CANCEL         │   │
│  └──────────────────┘ └──────────────────┘ └──────────────────┘   │
│                                                                      │
└────────────────────────────────────────────────────────────────────┘
```

---

## 🎨 DESIGN DETAILS

### Navigation Sidebar
```
┌──────────────┐
│ 🏢 AEROPORT  │  (Logo/Title)
│ ──────────── │
│              │
│ 📊 Dashboard │  (Primary color on hover)
│ ✈️  Flights   │
│ 🔍 Search    │
│ 📋 Reports   │
│ 📁 Archive   │
│ ⚙️  Settings  │
│              │
│ ──────────── │
│ 👤 Profile   │  (Bottom section)
│ 🚪 Logout    │
└──────────────┘
```

### Button Styles
```
PRIMARY BUTTON (M-Sport Red):
┌──────────────────────┐
│ ✓ SAVE FLIGHT        │  Background: #DC143C
│                      │  Text: #FFFFFF, Bold
│                      │  Hover: Brighter Red (#FF1744)
└──────────────────────┘

SECONDARY BUTTON (Tech Blue):
┌──────────────────────┐
│ 📄 EXPORT            │  Background: #00B4D8
│                      │  Text: #FFFFFF, Bold
└──────────────────────┘

DANGER BUTTON (Dark Red):
┌──────────────────────┐
│ 🗑️  DELETE FLIGHT     │  Background: #8B0000
│                      │  Text: #FFFFFF, Bold
│                      │  Hover: Darker (#660000)
└──────────────────────┘

OUTLINE BUTTON:
┌──────────────────────┐
│ ✕ CANCEL             │  Border: 2px #DC143C
│                      │  Background: Transparent
│                      │  Text: #DC143C
└──────────────────────┘
```

### Input Fields
```
STANDARD INPUT:
┌─────────────────────────────────┐
│ Flight Code                     │  Border: 1px #1a1f3a
│ FR-                             │  Bg: #1a1f3a
│                                 │  Text: #FFFFFF
└─────────────────────────────────┘

FOCUSED INPUT:
┌─────────────────────────────────┐
│ Flight Code                     │  Border: 2px #DC143C
│ FR-                             │  Bg: #1a1f3a
│ |                               │  Box-shadow: 0 0 12px rgba(220,20,60,0.3)
└─────────────────────────────────┘
```

### Data Tables
```
Header Row: Dark Charcoal (#1a1f3a) with Light Gray text
Even Rows: #0A0E27 (primary background)
Odd Rows: #1a1f3a (slightly lighter for alternating)
Hover Row: #DC143C with 20% opacity overlay
Selected Row: #DC143C with 30% opacity overlay
Borders: Subtle #444 dividers
```

### Card/Box Elements
```
STAT CARD:
┌────────────────────┐
│ 📊 ACTIVE FLIGHTS  │  Background: Gradient #1a1f3a → #0A0E27
│                    │  Border: 1px #DC143C (left border only)
│                    │  Border-radius: 8px
│                    │  Padding: 16px
│ 42                 │  Shadow: 0 8px 32px rgba(0,0,0,0.3)
└────────────────────┘
```

---

## 🎬 Animations & Transitions

```
TRANSITIONS:
- All state changes: 200-300ms ease-in-out
- Button hover: 150ms ease
- Panel slide: 300ms cubic-bezier(0.4, 0, 0.2, 1)
- Color change: 200ms linear

HOVER EFFECTS:
- Buttons: Slight elevation (2-4px shadow increase)
- Cards: Subtle color shift + shadow
- Navigation items: #DC143C highlight with animation
```

---

## 📱 Premium Features

✨ **Luxurious Polish**:
- Smooth transitions on all interactions
- Glass-morphism cards (semi-transparent + blur)
- Gradient overlays on hover
- Perfect spacing and alignment
- Premium typography hierarchy
- Responsive grid layout
- Dark mode optimized

🎯 **User Experience**:
- Clear visual hierarchy
- Intuitive navigation
- Consistent component design
- Accessible color contrast
- Professional, premium aesthetic
- Easy to scan and read

🔐 **Professional Features**:
- Real-time status indicators
- Professional data visualization
- Advanced filtering & search
- Comprehensive reporting
- Secure audit trails
