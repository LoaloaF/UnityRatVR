import os
import glob
import json

import numpy as np

import networkx as nx
import matplotlib
import matplotlib.pyplot as plt

REMAIN_IN_STATE_GUID = None

def getGUIDfromFileline(l):
    if ", " not in l:
        return []
    return l.split(", ")[-2].split(": ")[-1]

def get_colors(n, cmap_name='Set3'):
    cmap = matplotlib.colormaps.get_cmap(cmap_name)
    return cmap(np.arange(n))

def processAssetFileLines(asset, all_lines):
    # check if the asset is of type `state`
    stateID_line = [l for l in all_lines if "stateID:" in l]
    if stateID_line:
        asset["assetType"] = "state"
        asset["stateID"] = int(stateID_line[0].split(" ")[-1].strip())
        asset["paradigm"] = int(asset["name"][1:5])//100 *100
        try:
            name_stateID = int(asset["name"][1:5])
            if asset["stateID"] != name_stateID:
                print(f'\033[1;33;40m WARNING: Mismatch between stateID '
                        f'{asset["stateID"]} and name {asset["name"]} \033[0m')
        except Exception as e:
            print(e)
        
        # get the actions assciated with the state
        idx_actions_header_line = [i for i, l in enumerate(all_lines) 
                                    if "Action:" in l][0]
        actions_guids = []
        for l in all_lines[idx_actions_header_line+1:]:
            if l.startswith("  - "):
                actions_guids.append(getGUIDfromFileline(l))
            else:
                break
            asset["actions_guids"] = actions_guids

        # get the transitions assciated with the state
        idx_transitions_header_line = [i for i, l in enumerate(all_lines) 
                                        if "Transitions:" in l][0]
        transitions_guids = []
        for l in all_lines[idx_transitions_header_line+1:]:
            if l.startswith("  - "):
                transitions_guids.append(getGUIDfromFileline(l))
            else:
                break
        asset["transitions_guids"] = transitions_guids
        
    elif [l for l in all_lines if "Decision:" in l]:
        asset["assetType"] = "transition"
        
        decision_line = [l for l in all_lines if "Decision:" in l][0]
        asset["decision"] = getGUIDfromFileline(decision_line)
        truestate_line = [l for l in all_lines if "TrueState:" in l][0]
        asset["truestate"] = getGUIDfromFileline(truestate_line)
        falsestate_line = [l for l in all_lines if "FalseState:" in l][0]
        asset["falsestate"] = getGUIDfromFileline(falsestate_line)
    
    elif [l for l in all_lines if "switchDescription:" in l]:
        asset["assetType"] = "decision"
        description_line = [l for l in all_lines if "switchDescription:" in l][0]
        asset["switchDescription"] = description_line.split(": ")[-1].strip()
    else:
        asset["assetType"] = "action"
    return asset
        
        
def extractFromFiles(assets, assetfiles, assetmetafiles, path, full_path):
    for asfname, asmetafname in zip(sorted(assetfiles), sorted(assetmetafiles)):
        asset = {"path": path}
        
        with open(os.path.join(full_path, asmetafname), 'r') as f:
            guid_line = [l for l in f.readlines() if "guid:" in l][0]
            asset["guid"] = guid_line.split(" ")[-1].strip()
        
        with open(os.path.join(full_path, asfname), 'r') as f:
            all_lines = f.readlines()
            
        name_line = [l for l in all_lines if "m_Name:" in l][0]
        asset["name"] = name_line.split(" ")[-1].strip()
        if asset["name"].endswith("RemainInState"):
            global REMAIN_IN_STATE_GUID
            REMAIN_IN_STATE_GUID = asset["guid"]
            return {}
        asset = processAssetFileLines(asset, all_lines)
        assets.append(asset)
            
def extractAssets(PATH):
    def procress_dir(path):
        print("procress_dir with path: ", path)
        full_path = os.path.join(PATH, path)
        dirs = [f for f in os.listdir(full_path) 
                if os.path.isdir(os.path.join(full_path, f))]
        
        assetfiles = [f for f in os.listdir(full_path) 
                      if os.path.join(full_path, f).endswith(".asset")]
        assetmetafiles = [f for f in os.listdir(full_path) 
                          if os.path.join(full_path, f).endswith(".asset.meta")]
        print(f"\t{len(assetfiles)} asset files")
        extractFromFiles(assets, assetfiles, assetmetafiles, path, full_path)
        print(f"\tFound {len(dirs)} subdirs:")
        for d in dirs:
            procress_dir(os.path.join(path, d))
    
    assets = []
    # fill assets with all the assets in the path
    procress_dir("./")
    
    states, transitions, decisions, actions = {}, {}, {}, {}
    for ass in assets:
        if ass["assetType"] == "state":
            states.update({ass.pop("guid"): ass})
        if ass["assetType"] == "transition":
            transitions.update({ass.pop("guid"): ass})
        if ass["assetType"] == "decision":
            decisions.update({ass.pop("guid"): ass})
        if ass["assetType"] == "action":
            actions.update({ass.pop("guid"): ass})

    print(f"\n\nStates:\n {', '.join([v['name'] for v in list(states.values())])}"
          f"\n\nTransitions:\n {', '.join([v['name'] for v in list(transitions.values())])}"
          f"\n\nDecisions:\n {', '.join([v['name'] for v in list(decisions.values())])}"
          f"\n\nActions:\n {', '.join([v['name'] for v in list(actions.values())])}"
          )
    
    with open('fsm_states.json', 'w') as f:
        json.dump(states, f, indent=2)
    with open('fsm_transitions.json', 'w') as f:
        json.dump(transitions, f, indent=2)
    with open('fsm_decisions.json', 'w') as f:
        json.dump(decisions, f, indent=2)
    with open('fsm_actions.json', 'w') as f:
        json.dump(actions, f, indent=2)
    
    
    # print("\n\nS`TATES")
    # print(json.dumps(states, indent=2))
    # print("\n\nTRANSITIONS")
    # print(json.dumps(transitions, indent=2))
    # print("\n\nDecicions")
    # print(json.dumps(decisions, indent=2))
    # print("\n\nACTIONS")
    # print(json.d`umps(actions, indent=2))
    
    for state in states.values():
        print(state["name"])
        if state["name"].startswith("P"):
            which_paradigm = state["paradigm"]
            print(which_paradigm)
            if which_paradigm == -100:
                # TODO FIX
                continue
            paradigm_states = {guid: state for guid, state in states.items() 
                               if state['paradigm']==which_paradigm}
            visualizeFSM(paradigm_states, transitions, decisions, actions, paradigm=which_paradigm)


def visualizeFSM(states, transitions, decisions, actions, paradigm):
    G = nx.DiGraph()
    colors = get_colors(10)
    
    node_colors = []
    node_lbls = {}
    edge_lbls = {}
    # Add all nodes first
    for state_guid, state in states.items():
        int_state_guid = int(state_guid, 16)
        G.add_node(int_state_guid)
        
        # populate color and name for this node
        if state["name"].startswith("P"):
            node_colors.append('white')
        else:
            node_colors.append(colors[state["paradigm"]//100])
        node_lbls.update({int_state_guid: state["name"].replace("_","\n")})
    
    # Add the edges (trnasitions) associated with each state
    for state_guid, state in states.items():
        int_state_guid = int(state_guid, 16)
        
        for to_guid in state["transitions_guids"]:
            truestate_guid = transitions[to_guid].get("truestate")
            G.add_edge(int_state_guid, int(truestate_guid, 16))
            
            # transition to true state of decision
            edge_lbl = (f"{transitions[to_guid]['name']}\n"
                        f"{decisions[transitions[to_guid].get('decision')]['name']}?")
            edge_lbls[(int_state_guid, int(truestate_guid, 16))] = edge_lbl
            
            # transition to false state of decision
            falsestate_guid = transitions[to_guid].get("falsestate")
            if falsestate_guid and falsestate_guid != REMAIN_IN_STATE_GUID:
                G.add_edge(int_state_guid, int(falsestate_guid, 16))
                edge_lbl = (f"{transitions[to_guid]['name']}\n"
                            f"{decisions[transitions[to_guid].get('decision')]['name']}==False?")
                edge_lbls[(int_state_guid, int(falsestate_guid, 16))] = edge_lbl

    # Draw the graph
    plt.figure(figsize=(10,10))
    pos = nx.circular_layout(G)  # Use shell layout
    nx.draw(G, pos, node_color=node_colors, labels=node_lbls, node_size=2500, 
            edge_cmap=plt.cm.Blues, font_size=10, edge_color='black', width=1.5, 
            with_labels=True)
    nx.draw_networkx_edge_labels(G, pos, edge_labels=edge_lbls, font_size=8, )
    plt.savefig(f"P{paradigm:04}_FSM.png")
    plt.show()
    
def main():
    PATH = "../../Assets/scripts/FSMAssets"
    extractAssets(PATH)
    
if __name__ == "__main__":
    main()
    
    
    