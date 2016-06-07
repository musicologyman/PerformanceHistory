import os
import re
import xml.etree.ElementTree as ET

xmlfiles = [x for x in os.listdir() if re.search(r"(?<!subscriptions)\.xml$", x)]

def listxmlfiles():
    for x in enumerate(xmlfiles):
        print('{0:2} {1}'.format(x[0], x[1]))


def listchildtags(parent):
    for c in parent:
        print(c.tag)


def getchildelement(parent, childtag, f):
    child = parent.find(childtag)
    if child is None:
        return None
    return f(child)


def loadxmlfile(filename):
    tree = ET.parse(filename)
    return tree.getroot()


def loadxmlfilebyindex(index):
    return loadxmlfile(xmlfiles[index])


def getdescendants(parent, path):
    def descendants():
        for descendant in parent.iterfind(path):
            yield descendant

    return descendants


def issubscription(parent):
    et = parent.find('.//eventType')
    if et.text != 'Subscription Season':
        return False
    return True


def itersubscriptions(programs):
    for program in programs.iterfind('program'):
        if not issubscription(program):
            continue
        yield program


class ElementException(Exception):

    def __init__(self, expression, message):
        self.expression = expression
        self.message = message


def makesubscriptions():
    return ET.Element('subscriptions')


def makesubscriptionstree(rootelement):
    if rootelement.tag == 'subscriptions':
        return ET.ElementTree(rootelement)
    raise(ElementException(rootelement.tag), 'The root element must be "subscriptions"')


def buildsubscriptionstree(subscriptions, filename):
    programs = loadxmlfile(filename)
    for subscription in itersubscriptions(programs):
        subscriptions.append(subscription)


def writesubscriptions(fileindex):
    subscriptions = makesubscriptions()
    filename = xmlfiles[fileindex]
    buildsubscriptionstree(subscriptions, filename)
    subscriptionstree = makesubscriptionstree(subscriptions)

    outputfilename = re.sub(r"\.xml$", ".subscriptions.xml", filename)
    subscriptionstree.write(outputfilename, "UTF-8")

