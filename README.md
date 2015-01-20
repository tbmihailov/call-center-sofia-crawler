# call-center-sofia-crawler
Crawler for signals from http://call.sofia.bg

##About call.sofia.bg
Call and signal reporting page of Sofia Municipality. People can register and report various problems in different categories like "Infrastructure", "Municipality software systems" etc.

Example:
http://call.sofia.bg/bg/Signal/Details?id=1234

##Goal
Goal of this project is crawling base signals data reported by citizens. Crawled data can be used in building text classifier that assign category to a simple signal report identified by title and description.

##Execute crawler

SofiaCallCenter.Crawler.exe [id_from] [it_to] [output.csv]
- id_from - start signal ID to crawl from
- it_to - end signal ID to crawl to
- output.csv - CSV output file with base signal fields

Example:
SofiaCallCenter.Crawler.exe 120 180 signals.csv

Downloads infromation for signals with ID from 120 to 180 and saves data in signals.csv
