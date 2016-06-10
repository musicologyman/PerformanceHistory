import os
import re
import xml.etree.ElementTree as ET


def getprogramspath():
    return os.path.expanduser("~/Repos/PerformanceHistory/Programs/")


def getsubscriptionspath():
    return os.path.expanduser("~/Repos/PerformanceHistory/Subscriptions/")


def getprogramfiles(directory):
    return [x for x in os.listdir(directory) if re.search(r"(?<!subscriptions)\.xml$", x)]


def getxmlfiles(directory):
    return [x for x in os.listdir(directory) if re.search(r"\.xml$", x)]


def listxmlfiles(xmlfiles):
    for x in enumerate(xmlfiles):
        print('{0:2} {1}'.format(x[0], x[1]))


def loadxmlfile(filename):
    tree = ET.parse(filename)
    return tree.getroot()


def loadxmlfilebyindex(index, xmlfiles):
    return loadxmlfile(xmlfiles[index])
