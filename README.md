# FirmSheet_AAU_ADE
Create Firm sheets for Carrier Routes and Return Merchandise. Scan a single barcode and generate the Arrival at Unit and Acceptable Delivery Event (ADE) scans for the Delivery Unit for all pieces associated with that firm sheet.
# Project Timeline & Gantt Chart

This Gantt chart and timeline document maps each milestone to the 6-7 month project schedule.

| Milestone                                        | Start       | End         | Duration  |
|--------------------------------------------------|-------------|-------------|-----------|
| Requirements & Planning                          | 10/01/2025  | 10/14/2025  | 2 weeks   |
| Technology Stack Selection & Deployment Decision | 10/15/2025  | 10/28/2025  | 2 weeks   |
| UI Development: Corporation Management           | 10/29/2025  | 11/25/2025  | 4 weeks   |
| Microservice: Event Delivery to IDS via IOS      | 11/26/2025  | 12/23/2025  | 4 weeks   |
| NoSQL Database Design & Implementation           | 12/24/2025  | 01/14/2026  | 3 weeks   |
| Kafka Consumer Zip/Barcode                       | 01/15/2026  | 02/11/2026  | 4 weeks   |
| Firmsheet Print View (SWI, Example)              | 02/12/2026  | 03/11/2026  | 4 weeks   |
| Testing, QA, Documentation & Deployment          | 03/12/2026  | 04/30/2026  | 7 weeks   |

## Gantt Chart Visualization

```mermaid
gantt
    title Project Gantt Chart
    dateFormat  YYYY-MM-DD
    section Planning
    Requirements & Planning                :active, reqplan, 2025-10-01, 2025-10-14
    Technology Stack Selection             :active, techstack, 2025-10-15, 2025-10-28
    section Development
    UI: Corporation Management             :active, uicorp, 2025-10-29, 2025-11-25
    Microservice: IDS Event Delivery       :active, idsios, 2025-11-26, 2025-12-23
    NoSQL Database Implementation          :active, nosql, 2025-12-24, 2026-01-14
    Kafka Consumer Zip/Barcode             :active, kafka, 2026-01-15, 2026-02-11
    Firmsheet Print View                   :active, printview, 2026-02-12, 2026-03-11
    section Closure
    Testing, QA, Deployment & Docs         :active, qa, 2026-03-12, 2026-04-30
```

---

### Notes
- Milestones are aligned with the available resources (2 senior developers).
- The Firmsheet Print View milestone references USPS Standard Work Instructions and the attached image for UI requirements.
- Each milestone can be tracked in GitHub for progress.

---
